using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Newtonsoft.Json;
using Shared.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class UserModel : BaseModel
{
    //const string ServerURL = "http://localhost:7000";
    //const string ServerURL = "http://realtime-game.japaneast.cloudapp.azure.com:7000";
    int userId; //�o�^���[�U�[ID
    string userName; //�o�^���[�U�[�l�[��
    string authToken; //�g�[�N��
    string password; //�o�^�p�X���[�h

    private static UserModel instance;
    public static UserModel Instance
    {        get { return instance; } }

    //���[�U�[ID�����[�J���t�@�C���ɕۑ�����
    public void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.authToken = this.authToken;
        saveData.userName = this.userName;
        
        saveData.userID = userId;
        
        string json = JsonConvert.SerializeObject(saveData);
        var writer = new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    //���[�U�[ID�����[�J���t�@�C������ǂݍ���
    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            return false;
        }

        var reader =
            new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        authToken = saveData.authToken;
        userId = saveData.userID;
        userName = saveData.userName;
        

        if (authToken == null)
        {

            StartCoroutine(Instance.CreateToken(result =>
            {
                SaveUserData();
            }));

        }

        return true;
    }

    //�g�[�N����������
    public IEnumerator CreateToken(Action<bool> responce)
    {
        var requestData = new
        {
            user_id = userId
        };
        string json = JsonConvert.SerializeObject(requestData);
        UnityWebRequest request = UnityWebRequest.Post(ServerURL + "users/createToken", json, "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            //�ʐM�����������Ƃ��A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response = JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);

            //�t�@�C���Ƀ��[�U�[ID��ۑ�
            userId= response.UserID;
            authToken = response.Authtoken;
            SaveUserData();
        }
        responce?.Invoke(request.result == UnityWebRequest.Result.Success);
    }

    public async UniTask<bool> RegistAsync(string name, string password)
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        var client = MagicOnionClient.Create<IUserService>(channel);
        try
        {//�o�^����
            userId = await client.RegistUserAsync(name, password);
            return true;
        } catch (RpcException e)
        {//�o�^�����ς�
            Debug.Log(e);
            return false;
        }
    }
}