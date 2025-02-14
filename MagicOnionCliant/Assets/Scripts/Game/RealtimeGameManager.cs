
////////////////////////////////////////////////////////////////
///
/// �Q�[���̏�Ԃ𑍍��Ǘ�����X�N���v�g
/// 
/// Aughter:�ؓc�W��
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
using DG.Tweening;	//DOTween���g���Ƃ��͂���using������
using UnityEngine.UI;
using MasicOnionServer00.Model.Entity;
using Cysharp.Threading.Tasks;
using Cinemachine;
public class RealtimeGameManager : MonoBehaviour
{
    [SerializeField] InputField inputField;         //ID�̓��̓t�B�[���h
    [SerializeField] GameObject[] characterPrefab;  //�v���C���[�̃v���n�u
    [SerializeField] GameObject enemyPrefab;        //�G�̃v���n�u
    [SerializeField] GameObject[] objectPrefab;     //�I�u�W�F�N�g���ׂẴv���n�u
    [SerializeField] RoomModel roomModel;           //RoomModel�N���X���g�p
    [SerializeField] GameObject leaveButton;        //�ގ��{�^��
    [SerializeField] GameObject joinButton;         //�����{�^��
    [SerializeField] GameObject menuButton;         //���j���[�{�^��
    [SerializeField] GameObject menuPannel;         //���j���[���
    [SerializeField] Text item00Text;               //�e�X�g�A�C�e���̐��\���e�L�X�g
    [SerializeField] Text goalText;                 //�S�[����\������e�L�X�g
    [SerializeField] GameObject Spawnpoint;         //�����|�W�V����
    [SerializeField] GameObject[] EnemySpawnpoint;  //�G�����|�W�V����
    JoinedUser joinedUser;                          //JoinedUser�N���X���g�p

    [SerializeField] GameObject image;               //�A�C�e���\���e�X�g�p

    private CinemachineVirtualCamera virtualCamera; //CinemachineVirtualCamera���g�p

    //���[�U�[ID�ƃQ�[���I�u�W�F�N�g�𓯎��Ɋi�[
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();
    Player player;                                  //�v���C���[�N���X���g�p


    public int snowCount = 0;       //��ʂ�����g��ꂽ��
    public int playerCount = 0;     //�v���C���[������g��ꂽ��
    public float snowball_speed;    //��ʂ̃X�s�[�h
    int num = 0;                    //�ҋ@����
    int enemyid = 0;                //�G������g��ꂽ��
    int enemyAlive = 0;             //�G�����̂��邩
    int objectid = 0;               //�I�u�W�F�N�g������g��ꂽ��
    bool isjoin = false;            //�������Ă��邩
    bool isMaster = false;          //�}�X�^�[�N���C�A���g��
    bool enemyAsync = false;        //�G�̓�����������
    bool mine = false;              //���g������
    public static int item000 = 0;   //�莝���A�C�e���̐�
    public float speed;


    /// <summary>
    /// ���֘A
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
    
    public static int item00 = 0;   //�莝���A�C�e���̐�

    public int Item00
    {
        get { return item00; }
    }

   public static GameObject[] objectSnow;

    /// <summary>
    /// �N�����P�񂾂����s����
    /// </summary>
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
        //�G��������������OnMoveUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnSpawnEnemy += this.OnSpawnEnemy;
        //�G��Id���󂯎��������OnIdAsyncEnemyr���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnIdAsyncEnemy += this.OnIdAsyncEnemy;
        //�G���ړ���������OnMoveUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnMovedEnemy += this.OnMoveEnemy;
        //�G�����j���ꂽ����OnMoveUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnExcusionedEnemy += this.OnExcusionEnemy;
        //�}�X�^�[�N���C�A���g���n
        roomModel.OnMasteredClient += this.OnMasterdClient;
        //�I�u�W�F�N�g��������������OnSpawnObject���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnSpawnObject += this.OnSpawnObject;
        //�I�u�W�F�N�g���ړ������Ƃ���OnMovedObject���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnMovedObject += this.OnMovedObject;

        //�v���C���[�p�̒Ǐ]�J������T��
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        audioSource=GetComponent<AudioSource>();

        ////���O�C�����[�U�[�œ�������
        //await JoinRoom();
    }

    //�ڑ��J�n
    public async void Connect()
    {
        //���O�C�����[�U�[�œ�������
        await JoinRoom();
    }

    //�G�𐶐�
    public async void EnemySpawn()
    {
        await roomModel.SpawnEnemyAsync(enemyPrefab.name, 
                                        EnemySpawnpoint[UnityEngine.Random.Range(0,4)].transform.position 
                                        + new Vector3(UnityEngine.Random.Range(-30, 30), 2.0f, UnityEngine.Random.Range(-30, 30)));
    }

    //�G��Id���M
    public async void EnemySetId(int enemyId)
    {
        await roomModel.EnemyIdAsync(enemyId);
    }

    //�G�����j
    public async void ExcusionEnemy(string enemyName)
    {
        await roomModel.ExcusionEnemyAsync(enemyName);
    }

    //�I�u�W�F�N�g�𐶐�
    public async void ObjectSpawn(string objectName, Vector3 pos, Quaternion rot, Vector3 fow)
    {
        await roomModel.ObjectSpawnAsync(objectName, pos, rot, fow);
    }

    //�I�u�W�F�N�g���ړ�
    public async void ObjectMove(string objectName,Vector3 pos,Quaternion rot)
    {
        await roomModel.ObjectMoveAsync(objectName,pos,rot);
    }

    //��������
    public async UniTask JoinRoom()
    {
        //����
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

    //�ގ�����
    public async void LeaveRoom()
    {
        //�ގ�
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

        //��ʑJ��
        Initiate.DoneFading();
        Initiate.Fade("Home", Color.black, 0.5f);
    }

    //�ړ���̍��W�𑗂�
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

        //�}�X�^�[�N���C�A���g�̂ݓG�𐶐�������
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
        {//�ڑ�ID�ƈ�v������
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


        characterList[user.ConnectionId] = characterObject; //�t�B�[���h�ŕێ�
        playerCount++;
        characterObject.name = "Player" + playerCount;

        if (enemyid>0 && enemyAsync == false)
        {
            //enemyId�𑗂�
            EnemySetId(enemyid);
        }

    }

    //���[�U�[���ޏo�����Ƃ��̏���
    public void OnLeavedUser(JoinedUser user)
    {
        if (user.ConnectionId == roomModel.ConnectionId)
        {//�������ގ������ꍇ
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

    //�}�X�^�[�N���C�A���g���n
    public void OnMasterdClient(JoinedUser user)
    {
        if (user.ConnectionId == roomModel.ConnectionId)
        {
            roomModel.IsMaster = true;
            InvokeRepeating("EnemySpawn", 8.0f, 8.0f);
        }
    }

    //�v���C���[�̈ړ�
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

    //�G�̈ړ�����
    public async void EnemyMoveAsync(string enemyName, Vector3 pos, Quaternion rot)
    {
        await roomModel.MoveEnemyAsync(enemyName, pos, rot);
    }

    //�I�u�W�F�N�g�̈ړ�����
    public async void MoveObjAsync(string objectName, Vector3 pos, Quaternion rot)
    {
        await roomModel.ObjectMoveAsync(objectName, pos, rot);
    }

    //�G�̈ړ���]�̒ʒm
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

    //�v���C���[���S�[���ɓ��B�����Ƃ�
    public void Escape()
    {

        //leaveButton.SetActive(true);
        goalText.text = "GOAL!!";
        audioSource.PlayOneShot(escapeSE);

        LeaveRoom();
    }

    //�v���C���[�Ƀ_���[�W
    public void Damege()
    {
        audioSource.PlayOneShot(damageSE);
    }

    //�v���C���[��HP��0
    public void GameOver()
    {

        leaveButton.SetActive(true);
        goalText.text = "GameOver!!";
        audioSource.PlayOneShot(gameOverSE);

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
        GameObject enemyObject = enemyPrefab;

        enemyObject.name = "Enemy" + enemyid++;

        Instantiate(enemyObject, pos, Quaternion.identity);

        enemyAlive++;

        enemyObject.transform.position = pos;
    }

    //�G��Id�������Ă���
    public void OnIdAsyncEnemy(int enemyId)
    {
        enemyid = enemyId;
    }

    //�I�u�W�F�N�g���������ꂽ��
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

    //�I�u�W�F�N�g����������
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

    //���j���[���J��
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

        item00Text.text = "�~" + item00.ToString();
    }

    //���j���[�����
    public void CloseMenu()
    {
        menuPannel.SetActive(false);
        item00 = 0;
    }

    //�A�C�e�����z�[���Ɍp��
    public static int Itemset()
    {
        return item000;
    }

    //�v���C���[�������Ă���
    public void Walking()
    {
        audioSource.PlayOneShot(walkSE);
    }

    //�v���C���[�������Ă���
    public void Running()
    {
        audioSource.PlayOneShot(runSE);
    }

    public void Hit()
    {
        audioSource.PlayOneShot(snowhitSE);
    }
}
