
////////////////////////////////////////////////////////////////
///
/// ゲームの状態を総合管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;	//DOTweenを使うときはこのusingを入れる
using UnityEngine.UI;
using MasicOnionServer00.Model.Entity;
using Cysharp.Threading.Tasks;
using Cinemachine;
public class RealtimeGameManager : MonoBehaviour
{
    [SerializeField] InputField inputField;         //IDの入力フィールド
    [SerializeField] GameObject[] characterPrefab;  //プレイヤーのプレハブ
    [SerializeField] GameObject enemyPrefab;        //敵のプレハブ
    [SerializeField] GameObject[] objectPrefab;     //オブジェクトすべてのプレハブ
    [SerializeField] RoomModel roomModel;           //RoomModelクラスを使用
    [SerializeField] GameObject leaveButton;        //退室ボタン
    [SerializeField] GameObject joinButton;         //入室ボタン
    [SerializeField] GameObject menuButton;         //メニューボタン
    [SerializeField] GameObject menuPannel;         //メニュー画面
    [SerializeField] Text item00Text;               //テストアイテムの数表示テキスト
    [SerializeField] Text goalText;                 //ゴールを表示するテキスト
    [SerializeField] GameObject Spawnpoint;         //生成ポジション
    [SerializeField] GameObject[] EnemySpawnpoint;  //敵生成ポジション
    JoinedUser joinedUser;                          //JoinedUserクラスを使用

    [SerializeField] GameObject image;               //アイテム表示テスト用

    private CinemachineVirtualCamera virtualCamera; //CinemachineVirtualCameraを使用

    //ユーザーIDとゲームオブジェクトを同時に格納
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();
    Player player;                                  //プレイヤークラスを使用


    public int snowCount = 0;       //雪玉が何回使われたか
    public int playerCount = 0;     //プレイヤーが何回使われたか
    public float snowball_speed;    //雪玉のスピード
    int num = 0;                    //待機時間
    int enemyid = 0;                //敵が何回使われたか
    int enemyAlive = 0;             //敵が何体いるか
    int objectid = 0;               //オブジェクトが何回使われたか
    bool isjoin = false;            //入室しているか
    bool isMaster = false;          //マスタークライアントか
    bool enemyAsync = false;        //敵の同期をしたか
    bool mine = false;              //自身か判定
    public static int item000 = 0;   //手持ちアイテムの数
    public float speed;


    /// <summary>
    /// 音関連
    /// </summary>
    AudioSource audioSource;


    //SE
    public AudioClip walkSE;
    public AudioClip runSE;
    public AudioClip snowhitSE;
    public AudioClip gameOverSE;
    public AudioClip escapeSE;
    public AudioClip damageSE;

   

    public int Item000
    {
        get { return item000; }
    }
    
    public static int item00 = 0;   //手持ちアイテムの数

    public int Item00
    {
        get { return item00; }
    }

   public static GameObject[] objectSnow;

    /// <summary>
    /// 起動時１回だけ実行する
    /// </summary>
    async void Start()
    {
        //接続
        await roomModel.ConnectAsync();
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
        //ユーザーが入室した時にOnJoinedUserメソッドを実行するよう、モデルに登録
        roomModel.OnJoinedUser += this.OnJoinedUser;
        //ユーザーが退室した時にOnLeavedUserメソッドを実行するよう、モデルに登録
        roomModel.OnLeavedUser += this.OnLeavedUser;
        //ユーザーが移動した時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnMoveCharacter += this.OnMoveCharacter;
        //敵が生成した時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnSpawnEnemy += this.OnSpawnEnemy;
        //敵のIdを受け取った時にOnIdAsyncEnemyrメソッドを実行するよう、モデルに登録
        roomModel.OnIdAsyncEnemy += this.OnIdAsyncEnemy;
        //敵が移動した時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnMovedEnemy += this.OnMoveEnemy;
        //敵が撃破された時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnExcusionedEnemy += this.OnExcusionEnemy;
        //マスタークライアント譲渡
        roomModel.OnMasteredClient += this.OnMasterdClient;
        //オブジェクトが生成した時にOnSpawnObjectメソッドを実行するよう、モデルに登録
        roomModel.OnSpawnObject += this.OnSpawnObject;
        //オブジェクトが移動したときにOnMovedObjectメソッドを実行するよう、モデルに登録
        roomModel.OnMovedObject += this.OnMovedObject;

        //プレイヤー用の追従カメラを探す
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        audioSource=GetComponent<AudioSource>();

        ////ログインユーザーで入室する
        //await JoinRoom();
    }

    //接続開始
    public async void Connect()
    {
        //ログインユーザーで入室する
        await JoinRoom();
    }

    //敵を生成
    public async void EnemySpawn()
    {
        await roomModel.SpawnEnemyAsync(enemyPrefab.name, 
                                        EnemySpawnpoint[UnityEngine.Random.Range(0,4)].transform.position 
                                        + new Vector3(UnityEngine.Random.Range(-30, 30), 2.0f, UnityEngine.Random.Range(-30, 30)));
    }

    //敵のId送信
    public async void EnemySetId(int enemyId)
    {
        await roomModel.EnemyIdAsync(enemyId);
    }

    //敵を撃破
    public async void ExcusionEnemy(string enemyName)
    {
        await roomModel.ExcusionEnemyAsync(enemyName);
    }

    //オブジェクトを生成
    public async void ObjectSpawn(string objectName, Vector3 pos, Quaternion rot, Vector3 fow)
    {
        await roomModel.ObjectSpawnAsync(objectName, pos, rot, fow);
    }

    //オブジェクトを移動
    public async void ObjectMove(string objectName,Vector3 pos,Quaternion rot)
    {
        await roomModel.ObjectMoveAsync(objectName,pos,rot);
    }

    //入室処理
    public async UniTask JoinRoom()
    {
        //入室
        int id;
        string pid = inputField.text;
        int.TryParse(pid, out id);
        if (id == 0) { await roomModel.JoinedAsync("sampleRoom", UserModel.Instance.userId); }
        else { await roomModel.JoinedAsync("sampleRoom", id); }

        joinButton.SetActive(false);
        inputField.gameObject.SetActive(false);

        InvokeRepeating("SendPos", 0.1f, 0.1f);
        //InvokeRepeating("EnemySpawn", 8.0f,8.0f);
        //await UniTask.Delay(TimeSpan.FromSeconds(3.0f));


    }

    //退室処理
    public async void LeaveRoom()
    {
        //退室
        CancelInvoke("SendPos");
        CancelInvoke("EnemySpawn");
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
        //joinButton.SetActive(true);
        isjoin = false;
        //goalText.text = "";

        for (int i = 0; i < player.itemPrefab.Length; i++)
        {
            if (player.itemPrefab[i] == null) { break; }
            if (player.itemPrefab[i].name == objectPrefab[2].name)
            {
                item000++;
            }
            else
            {

            }

        }

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
    }

    //移動先の座標を送る
    public async void SendPos()
    {
        //移動同期
        GameObject characterOblect = characterList[roomModel.ConnectionId];

        await roomModel.MoveAsync(characterOblect.transform.position, characterOblect.transform.rotation,
            characterOblect.GetComponent<Animator>().GetInteger("Speed"));
    }



    //ユーザーが入室したときの処理
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject;
        joinedUser = user;

        characterObject = Instantiate(characterPrefab[0]);//インスタンス生成

        //マスタークライアントのみ敵を生成させる
        if (joinedUser.IsMaster == true) 
        {
            enemyAsync = true;
            InvokeRepeating("EnemySpawn", 8.0f, 8.0f); 
        }
        else
        {
            enemyAsync= false;
        }

        if (user.ConnectionId == roomModel.ConnectionId)
        {//接続IDと一致したら
            player = characterObject.GetComponent<Player>();
            characterObject.GetComponent<Player>().isself = true;


            if (characterList.Count == 0)
            {
                characterObject.transform.position = new Vector3(0, 0.5f, 0);

            }
            else if (characterList.Count == 1)
            {
                characterObject.transform.position = new Vector3(-3, 0.5f, 0);
            }
            else
            {
                characterObject.transform.position = new Vector3(UnityEngine.Random.Range(-8, 8), 0, UnityEngine.Random.Range(-3, 3));
            }

            Transform character = characterObject.transform;
            virtualCamera.LookAt = character;
            virtualCamera.Follow = character;

            
        }


        characterList[user.ConnectionId] = characterObject; //フィールドで保持
        playerCount++;
        characterObject.name = "Player" + playerCount;

        if (enemyid>0 && enemyAsync == false)
        {
            //enemyIdを送る
            EnemySetId(enemyid);
        }

    }

    //ユーザーが退出したときの処理
    public void OnLeavedUser(JoinedUser user)
    {
        if (user.ConnectionId == roomModel.ConnectionId)
        {//自分が退室した場合
            foreach (var cube in characterList)
            {
                Destroy(cube.Value);
            }

        }
        else
        {
            Destroy(characterList[user.ConnectionId]);


        }


    }

    //マスタークライアント譲渡
    public void OnMasterdClient(JoinedUser user)
    {
        if (user.ConnectionId == roomModel.ConnectionId)
        {
            roomModel.IsMaster = true;
            InvokeRepeating("EnemySpawn", 8.0f, 8.0f);
        }
    }

    //プレイヤーの移動
    void OnMoveCharacter(JoinedUser user, Vector3 pos, Quaternion rot, int anim)
    {
        if (characterList.ContainsKey(user.ConnectionId))
        {
            GameObject characterObject = characterList[user.ConnectionId];

            characterObject.transform.DOLocalMove(pos, 0.1f).SetEase(Ease.Linear);
            characterObject.transform.DORotate(rot.eulerAngles, 0.1f);

            characterObject.GetComponent<Animator>().SetInteger("Speed", anim);
        }
    }

    //敵の移動同期
    public async void EnemyMoveAsync(string enemyName, Vector3 pos, Quaternion rot)
    {
        await roomModel.MoveEnemyAsync(enemyName, pos, rot);
    }

    //オブジェクトの移動同期
    public async void MoveObjAsync(string objectName, Vector3 pos, Quaternion rot)
    {
        await roomModel.ObjectMoveAsync(objectName, pos, rot);
    }

    //敵の移動回転の通知
    void OnMoveEnemy(string enemyName, Vector3 pos, Quaternion rot)
    {
        GameObject enemy = GameObject.Find(enemyName);

        if (enemy == null)
        {
            return;
        }

        //enemy.transform.DOLocalMove(pos,0.1f).SetEase(Ease.Linear);
        //enemy.transform.DORotate(rot.eulerAngles,0.1f);

        enemy.transform.position = pos;
        enemy.transform.rotation = rot;
    }

    void OnExcusionEnemy(string enemyName)
    {
        GameObject enemy = GameObject.Find(enemyName);

        if (enemy == null)
        {
            return;
        }

        Destroy(enemy);
        enemyAlive--;
    }

    //プレイヤーがゴールに到達したとき
    public void Escape()
    {

        //leaveButton.SetActive(true);
        goalText.text = "GOAL!!";
        audioSource.PlayOneShot(escapeSE);

        LeaveRoom();
    }

    //プレイヤーにダメージ
    public void Damege()
    {
        audioSource.PlayOneShot(damageSE);
    }

    //プレイヤーのHPが0
    public void GameOver()
    {

        leaveButton.SetActive(true);
        goalText.text = "GameOver!!";
        audioSource.PlayOneShot(gameOverSE);

    }

    //プレイヤーが攻撃
    public void Attack()
    {
        player.SlowSnow();
    }

    //プレイヤーが走る、歩く
    public void Run()
    {
        if (player.run == false)
        {
            player.run = true;
        }
        else
        {
            player.run = false;
        }

    }

    //敵が出現したとき
    public void OnSpawnEnemy(string enemyName, Vector3 pos)
    {
        GameObject enemyObject = enemyPrefab;

        enemyObject.name = "Enemy" + enemyid++;

        Instantiate(enemyObject, pos, Quaternion.identity);

        enemyAlive++;

        enemyObject.transform.position = pos;
    }

    //敵のIdが送られてきた
    public void OnIdAsyncEnemy(int enemyId)
    {
        enemyid = enemyId;
    }

    //オブジェクトが生成されたら
    public void OnSpawnObject(Guid connectionId, string objectName, Vector3 pos, Quaternion rot, Vector3 fow)
    {


        objectSnow = objectPrefab;

        int objectid = 0;

        if (connectionId != roomModel.ConnectionId) { objectid = 1; }
        if (objectName == "Item") { objectid = 2; }



        Instantiate(objectSnow[objectid], pos, rot);



        objectSnow[objectid].transform.position = pos;
        objectSnow[objectid].transform.rotation = rot;

        if (objectid > 1) { return; }
        Snow snow = objectSnow[objectid].GetComponent<Snow>();


        Rigidbody rb = objectSnow[objectid].GetComponent<Rigidbody>();

        rb.velocity = fow * speed;
        rb.AddForce(fow * speed);

        snow.MoveSnow(pos,fow);
    }

    //オブジェクトが動いたら
    public void OnMovedObject(string objectName, Vector3 pos, Quaternion rot)
    {
        GameObject objectSnow = GameObject.Find(objectName);

        if (objectSnow == null)
        {
            return;
        }

        //enemy.transform.DOLocalMove(pos,0.1f).SetEase(Ease.Linear);
        //enemy.transform.DORotate(rot.eulerAngles,0.1f);

        objectSnow.transform.position = pos;
        objectSnow.transform.rotation = rot;
    }

    //メニューを開く
    public void OpenMenu()
    {
        menuPannel.SetActive(true);
        image.SetActive(true);

        for(int i=0;i<player.itemPrefab.Length;i++)
        {
            if (player.itemPrefab[i] == null) { break; }
            if (player.itemPrefab[i].name == objectPrefab[2].name)
            {
                item00++;
            }
            else
            {
                
            }
            
        }

        item00Text.text = "×" + item00.ToString();
    }

    //メニューを閉じる
    public void CloseMenu()
    {
        menuPannel.SetActive(false);
        item00 = 0;
    }

    //アイテムをホームに継承
    public static int Itemset()
    {
        return item000;
    }

    //プレイヤーが歩いている
    public void Walking()
    {
        audioSource.PlayOneShot(walkSE);
    }

    //プレイヤーが走っている
    public void Running()
    {
        audioSource.PlayOneShot(runSE);
    }

    public void Hit()
    {
        audioSource.PlayOneShot(snowhitSE);
    }
}
