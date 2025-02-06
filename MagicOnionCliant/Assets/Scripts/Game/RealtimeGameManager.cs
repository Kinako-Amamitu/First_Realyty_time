
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
    int objectid = 0;               //オブジェクトが何回使われたか
    bool isjoin = false;            //入室しているか
    bool mine = false;              //自身か判定
    public static int item000 = 0;   //手持ちアイテムの数

    public int Item000
    {
        get { return item000; }
    }
    
    public static int item00 = 0;   //手持ちアイテムの数

    public int Item00
    {
        get { return item00; }
    }

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
        //敵が移動した時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnSpawnEnemy += this.OnSpawnEnemy;
        //敵が移動した時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnMovedEnemy += this.OnMoveEnemy;
        //敵が撃破された時にOnMoveUserメソッドを実行するよう、モデルに登録
        roomModel.OnExcusionedEnemy += this.OnExcusionEnemy;
        //マスタークライアント譲渡
        roomModel.OnMasteredClient += this.OnMasterdClient;
        //敵が生成した時にOnSpawnObjectメソッドを実行するよう、モデルに登録
        roomModel.OnSpawnObject += this.OnSpawnObject;
        //敵が移動したときにOnMovedObjectメソッドを実行するよう、モデルに登録
        roomModel.OnMovedObject += this.OnMovedObject;

        //プレイヤー用の追従カメラを探す
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    //敵を生成
    public async void EnemySpawn()
    {
        await roomModel.SpawnEnemyAsync(enemyPrefab.name, new Vector3(UnityEngine.Random.Range(-8, 8), 2.0f, UnityEngine.Random.Range(-3, 3)));
    }

    //敵を撃破
    public async void ExcusionEnemy(string enemyName)
    {
        await roomModel.ExcusionEnemyAsync(enemyName);
    }

    //オブジェクトを生成
    public async void ObjectSpawn(string objectName, Vector3 pos, Quaternion rot)
    {
        await roomModel.ObjectSpawnAsync(objectName, pos, rot);
    }

    //入室処理
    public async void JoinRoom()
    {
        // 入室
        int id;
        string pid = inputField.text;
        if (pid == null) { return; }
        int.TryParse(pid, out id);
        if (id <= 0) { return; }
        await roomModel.JoinedAsync("sampleRoom", id);

        joinButton.SetActive(false);

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
        joinButton.SetActive(true);
        isjoin = false;
        goalText.text = "";

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
        if (joinedUser.IsMaster == true) { InvokeRepeating("EnemySpawn", 8.0f, 8.0f); }

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
    }

    //プレイヤーがゴールに到達したとき
    public void Escape()
    {

        leaveButton.SetActive(true);
        goalText.text = "GOAL!!";

    }

    //デバッグ用プレイヤーにダメージ
    public void Damege()
    {

        player.UpdateHP();
    }

    //プレイヤーのHPが0
    public void GameOver()
    {

        leaveButton.SetActive(true);
        goalText.text = "GameOver!!";

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



        enemyObject.transform.position = pos;
    }

    //オブジェクトが生成されたら
    public void OnSpawnObject(Guid connectionId, string objectName, Vector3 pos, Quaternion rot)
    {


        GameObject[] objectSnow = objectPrefab;

        int objectid = 0;

        if (connectionId != roomModel.ConnectionId) { objectid = 1; }
        if (objectName == "Item") { objectid = 2; }



        Instantiate(objectSnow[objectid], pos, rot);



        objectSnow[objectid].transform.position = pos;
        objectSnow[objectid].transform.rotation = rot;

        if (objectid > 1) { return; }
        Snow snow = objectSnow[objectid].GetComponent<Snow>();




        snow.MoveSnow();
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

}
