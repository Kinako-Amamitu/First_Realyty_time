using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] InputField InputField;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] GameObject leaveButton;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();
    async void Start()
    {
        //���[�U�[��������������OnJoinedUser���\�b�h�����s����悤�A���f���ɓo�^
        roomModel.OnJoinedUser += this.OnJoinedUser;
        //�ڑ�
        await roomModel.ConnectAsync();
    }
    public async void JoinRoom() {
        // ����
        await roomModel.JoinedAsync("sampleRoom", 1);
        leaveButton.SetActive(true);
    }
    public async void LeaveRoom()
    {
        //�ގ�
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
    }
    //���[�U�[�����������Ƃ��̏���
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab);//�C���X�^���X����
        characterObject.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), UnityEngine.Random.Range(-3, 3), 0);
        characterList[user.ConnectionId] = characterObject; //�t�B�[���h�ŕێ�

    }

    public void OnLeavedUser(JoinedUser user)
    {
        Destroy(characterList[user.ConnectionId]);
    }
}
