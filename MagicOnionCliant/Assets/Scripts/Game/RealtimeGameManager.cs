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
    [SerializeField] InputField inputField;
    [SerializeField] GameObject[] characterPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject joinButton;
    [SerializeField] Text goalText;
    [SerializeField] GameObject Spawnpoint;
    JoinedUser joinedUser;

    private CinemachineVirtualCamera virtualCamera;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();
    Player player;

    public int snowCount = 0;
    public int playerCount = 0;
    int num = 0;
    int enemyid = 0;
    bool isjoin = false;
    bool mine = false;
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

        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

    }

    async void Update()
    {
        
    }


    //public async void RespownEnemy()
    //{
    //    // num++;
    //    //if (num % 5000 == 0)
    //    //{
    //    GameObject enemy;

    //    await UniTask.Delay(TimeSpan.FromSeconds(3.0f));

    //    enemy = Instantiate(enemyPrefab);

    //    enemy.name = "Enemy" + enemyid++;
    //    enemy.transform.position = new Vector3(UnityEngine.Random.Range(-8, 8), 2.0f, UnityEngine.Random.Range(-3, 3));

       
    //    // }
    //}

    public async void EnemySpawn()
    {
        await roomModel.SpawnEnemyAsync(enemyPrefab.name, new Vector3(UnityEngine.Random.Range(-8, 8), 2.0f, UnityEngine.Random.Range(-3, 3)));
    }


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

        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
    }


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

        

        //player = characterObject.GetComponent<Player>(); //Unityのプレイヤー情報を取得

        if (user.ConnectionId == roomModel.ConnectionId)
        {
            player=characterObject.GetComponent<Player>();
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
            characterObject.name="Player"+playerCount;

        /*if (user.ConnectionId == roomModel.ConnectionId)
        {
            
           // player.Me();
            
        }
        else
        {

           // player.NotMe();
            player.enabled = false;
        }*/

        
    }

    //ユーザーが退出したときの処理
    public void OnLeavedUser(JoinedUser user)
    {
        if (user.ConnectionId == roomModel.ConnectionId)
        {
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

    //プレイヤーの移動
    void OnMoveCharacter(JoinedUser user, Vector3 pos, Quaternion rot, int anim)
    {
        if(characterList.ContainsKey(user.ConnectionId)) 
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

    //敵の移動回転の通知
    void OnMoveEnemy(string enemyName, Vector3 pos, Quaternion rot)
    {
        GameObject enemy = GameObject.Find(enemyName);

        //enemy.transform.DOLocalMove(pos,0.1f).SetEase(Ease.Linear);
        //enemy.transform.DORotate(rot.eulerAngles,0.1f);

        enemy.transform.position = pos;
        enemy.transform.rotation = rot;
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
        GameObject enemyObject=enemyPrefab;

        Instantiate(enemyObject, pos, Quaternion.identity);

        enemyObject.name = "Enemy" + enemyid++;

        enemyObject.transform.position = pos;
    }
}
