using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel : BaseModel
{
    const string ServerURL = "http://localhost:7000";
    private int userId; //�o�^���[�U�[ID
    public async UniTask<bool>RegistAsync(string name)
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL);
        var client = MagicOnionClient.Create<IUserService>(channel);
        try
        {//�o�^����
            userId = await client.RegistUserAsync(name);
            return true;
        }catch(RpcException e)
        {//�o�^�����ς�
            Debug.Log(e);
            return false;
        }
    }
}
