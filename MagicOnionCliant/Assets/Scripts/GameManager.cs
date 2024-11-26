using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject joinButton;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();
    async void Start()
    {
        //Component��������悤�ɂ���
        inputField = inputField.GetComponent<InputField>();
        //���[�U�[��������������OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnJoinedUser += this.OnJoinedUser;
        roomModel.OnLeavedUser += this.OnLeavedUser;
        //�ڑ�
        await roomModel.ConnectAsync();
    }
    public async void JoinRoom() {
        // ����
        int id;
        string pid = inputField.text;
        int.TryParse(pid, out id);
        await roomModel.JoinedAsync("sampleRoom", id);
        leaveButton.SetActive(true);
        joinButton.SetActive(false);
    }
    public async void LeaveRoom()
    {
        //�ގ�
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
        joinButton.SetActive(true);
    }
    //���[�U�[�����������Ƃ��̏���
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab);//�C���X�^���X����
        characterObject.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), UnityEngine.Random.Range(-3, 3), 0);
        characterList[user.ConnectionId] = characterObject; //�t�B�[���h�ŕێ�

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
}
