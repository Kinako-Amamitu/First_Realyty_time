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
        //ユーザーが入室した時にOnJoinedUserメソッドを実行するよう、モデルに登録
        roomModel.OnJoinedUser += this.OnJoinedUser;
        //接続
        await roomModel.ConnectAsync();
    }
    public async void JoinRoom() {
        // 入室
        await roomModel.JoinedAsync("sampleRoom", 1);
        leaveButton.SetActive(true);
    }
    public async void LeaveRoom()
    {
        //退室
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
    }
    //ユーザーが入室したときの処理
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab);//インスタンス生成
        characterObject.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), UnityEngine.Random.Range(-3, 3), 0);
        characterList[user.ConnectionId] = characterObject; //フィールドで保持

    }

    public void OnLeavedUser(JoinedUser user)
    {
        Destroy(characterList[user.ConnectionId]);
    }
}
