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
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
        //ユーザーが入室した時にOnJoinedUserメソッドを実行するよう、モデルに登録
        roomModel.OnJoinedUser += this.OnJoinedUser;
        roomModel.OnLeavedUser += this.OnLeavedUser;
        //接続
        await roomModel.ConnectAsync();
    }
    public async void JoinRoom() {
        // 入室
        int id;
        string pid = inputField.text;
        int.TryParse(pid, out id);
        await roomModel.JoinedAsync("sampleRoom", id);
        leaveButton.SetActive(true);
        joinButton.SetActive(false);
    }
    public async void LeaveRoom()
    {
        //退室
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
        joinButton.SetActive(true);
    }
    //ユーザーが入室したときの処理
    private void OnJoinedUser(JoinedUser user)
    {
        GameObject characterObject = Instantiate(characterPrefab);//インスタンス生成
        characterObject.transform.position = new Vector3(UnityEngine.Random.Range(-8,8), UnityEngine.Random.Range(-3, 3), 0);
        characterList[user.ConnectionId] = characterObject; //フィールドで保持

    }

    //ユーザーが退出したときの処理
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
