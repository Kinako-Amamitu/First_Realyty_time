using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;	//DOTween���g���Ƃ��͂���using������
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
        //�ڑ�
        await roomModel.ConnectAsync();
        //Component��������悤�ɂ���
        inputField = inputField.GetComponent<InputField>();
        //���[�U�[��������������OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnJoinedUser += this.OnJoinedUser;
        //���[�U�[���ގ���������OnLeavedUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnLeavedUser += this.OnLeavedUser;
        //���[�U�[���ړ���������OnMoveUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnMoveCharacter += this.OnMoveCharacter;
        //�G���ړ���������OnMoveUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnSpawnEnemy += this.OnSpawnEnemy;
        //�G���ړ���������OnMoveUser���\�b�h�����s����悤�A���f���ɓo�^
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
        // ����
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
        //�ގ�
        CancelInvoke("SendPos");
        CancelInvoke("EnemySpawn");
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
        joinButton.SetActive(true);
        isjoin = false;
        goalText.text = "";

        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
    }


    public async void SendPos()
    {
        //�ړ�����
        GameObject characterOblect = characterList[roomModel.ConnectionId];
        
        await roomModel.MoveAsync(characterOblect.transform.position, characterOblect.transform.rotation,
            characterOblect.GetComponent<Animator>().GetInteger("Speed"));
    }



    //���[�U�[�����������Ƃ��̏���
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject;
        joinedUser = user;

        characterObject = Instantiate(characterPrefab[0]);//�C���X�^���X����

        

        //player = characterObject.GetComponent<Player>(); //Unity�̃v���C���[�����擾

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
       

        characterList[user.ConnectionId] = characterObject; //�t�B�[���h�ŕێ�
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

    //���[�U�[���ޏo�����Ƃ��̏���
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

    //�v���C���[�̈ړ�
    void OnMoveCharacter(JoinedUser user, Vector3 pos, Quaternion rot, int anim)
    {
        if(characterList.ContainsKey(user.ConnectionId)) 
        {
            GameObject characterObject = characterList[user.ConnectionId];

            characterObject.transform.DOLocalMove(pos, 0.1f).SetEase(Ease.Linear);
            characterObject.transform.DORotate(rot.eulerAngles, 0.1f);
        }
    }

    //�G�̈ړ�����
    public async void EnemyMoveAsync(string enemyName, Vector3 pos, Quaternion rot)
    {
        await roomModel.MoveEnemyAsync(enemyName, pos, rot);
    }

    //�G�̈ړ���]�̒ʒm
    void OnMoveEnemy(string enemyName, Vector3 pos, Quaternion rot)
    {
        GameObject enemy = GameObject.Find(enemyName);

        //enemy.transform.DOLocalMove(pos,0.1f).SetEase(Ease.Linear);
        //enemy.transform.DORotate(rot.eulerAngles,0.1f);

        enemy.transform.position = pos;
        enemy.transform.rotation = rot;
    }



    //�v���C���[���S�[���ɓ��B�����Ƃ�
    public void Escape()
    {

        leaveButton.SetActive(true);
        goalText.text = "GOAL!!";

    }

    //�f�o�b�O�p�v���C���[�Ƀ_���[�W
    public void Damege()
    {

        player.UpdateHP();
    }

    //�v���C���[��HP��0
    public void GameOver()
    {

        leaveButton.SetActive(true);
        goalText.text = "GameOver!!";

    }

    //�v���C���[���U��
    public void Attack()
    {
        player.SlowSnow();
    }

    //�v���C���[������A����
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

    //�G���o�������Ƃ�
    public void OnSpawnEnemy(string enemyName, Vector3 pos)
    {
        GameObject enemyObject=enemyPrefab;

        Instantiate(enemyObject, pos, Quaternion.identity);

        enemyObject.name = "Enemy" + enemyid++;

        enemyObject.transform.position = pos;
    }
}
