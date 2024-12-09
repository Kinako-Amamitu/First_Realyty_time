using Shared.Interfaces.StreamingHubs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] RoomModel roomModel;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject joinButton;


    // Start is called before the first frame update
    async void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
        //ユーザーが入室した時にOnJoinedUserメソッドを実行するよう、モデルに登録
        roomModel.OnJoinedUser += this.OnJoinedUser;
        //ユーザーが退室した時にOnLeavedUserメソッドを実行するよう、モデルに登録
        roomModel.OnLeavedUser += this.OnLeavedUser;

        //接続
        await roomModel.ConnectAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        leaveButton.SetActive(true);
        //InvokeRepeating("SendPos", 0.1f, 0.1f);
    }
    public async void LeaveRoom()
    {
        //退室
        //CancelInvoke("SendPos");
        await roomModel.LeaveAsync();
        leaveButton.SetActive(false);
        joinButton.SetActive(true);
    }

    //ユーザーが入室したときの処理
    private void OnJoinedUser(JoinedUser user)
    {
        
    }

    //ユーザーが退出したときの処理
    public void OnLeavedUser(JoinedUser user)
    {

    }

    //Map１に遷移する
    public void Map1Fade()
    {
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Map1", Color.black, 0.5f);
    }

    //装備強化
    public void Weponup()
    {
        
    }

    //ランキング表示
    public void Ranking() 
    {

    }

    //ガチャ
    public void ItemGetChallenge()
    {

    }
}
