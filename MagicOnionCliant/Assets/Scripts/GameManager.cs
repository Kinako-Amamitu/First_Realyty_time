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
public class GameManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] GameObject[] characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject joinButton;
    [SerializeField] Text goalText;
    JoinedUser joinedUser;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();
    Player player;

    bool isjoin=false;
    bool mine = false;
    async void Start()
    {
        //Component��������悤�ɂ���
        inputField = inputField.GetComponent<InputField>();
        //���[�U�[��������������OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnJoinedUser += this.OnJoinedUser;
        roomModel.OnLeavedUser += this.OnLeavedUser;
        roomModel.OnMoveCharacter += this.OnMoveCharacter;
        //�ڑ�
        await roomModel.ConnectAsync();
    }



    public async void JoinRoom() {
        // ����
        int id;
        string pid = inputField.text;
        if (pid == null) { return; }
        int.TryParse(pid, out id);
        if(id <= 0) { return; }
        await roomModel.JoinedAsync("sampleRoom", id);
        
        joinButton.SetActive(false);
        InvokeRepeating("SendPos", 0.1f, 0.1f);
    }
    public async void LeaveRoom()
    {
        //�ގ�
        CancelInvoke("SendPos");
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
        joinButton.SetActive(true);
        isjoin = false;
        goalText.text = "";
    }

    public async void Escape()
    {
        await roomModel.GoalAsync();
    }

    public async void SendPos()
    {
        //�ړ�����
        GameObject characterOblect = characterList[roomModel.ConnectionId];
        await roomModel.MoveAsync(characterOblect.transform.position,characterOblect.transform.rotation);
    }

    //���[�U�[�����������Ƃ��̏���
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject;
        joinedUser = user;
        if (user.ConnectionId == roomModel.ConnectionId)
        {
            characterObject = Instantiate(characterPrefab[0]);//�C���X�^���X����
        }
        else
        {
            characterObject = Instantiate(characterPrefab[1]);//�C���X�^���X����
        }
        

       player= characterObject.GetComponent<Player>(); //Unity�̃v���C���[�����擾

        if (characterList.Count==0)
        {
            characterObject.transform.position = new Vector3(0, 0, 0);

        }
        else if(characterList.Count==1)
        {
            characterObject.transform.position = new Vector3(-3, 0, 0);
        }
        else
        {
            characterObject.transform.position = new Vector3(UnityEngine.Random.Range(-8, 8), UnityEngine.Random.Range(-3, 3), 0);
        }
        characterList[user.ConnectionId] = characterObject; //�t�B�[���h�ŕێ�

        if (user.ConnectionId == roomModel.ConnectionId)
        {
            player.Me();
        }
        else
        {
            player.NotMe();
        }
    }

    //���[�U�[���ޏo�����Ƃ��̏���
    public void OnLeavedUser(JoinedUser user)
    {
        if(user.ConnectionId==roomModel.ConnectionId)
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
    void OnMoveCharacter(JoinedUser user,Vector3 pos,Quaternion rot)
    {
        GameObject characterObject = characterList[user.ConnectionId];

        characterObject.transform.DOLocalMove(pos, 0.1f);
        characterObject.transform.DORotateQuaternion(rot,0.1f);
    }

    //�v���C���[���S�[���ɓ��B�����Ƃ�
    public void OnEscapeCharacter(JoinedUser user)
    {
        if  (roomModel.ConnectionId == user.ConnectionId)
        {
            leaveButton.SetActive(true);
            goalText.text = "GOAL!!";

        }            
           
    }
}
