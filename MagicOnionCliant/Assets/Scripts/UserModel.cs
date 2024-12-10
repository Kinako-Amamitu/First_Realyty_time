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
    private int userId; //ìoò^ÉÜÅ[ÉUÅ[ID
    public async UniTask<bool>RegistAsync(string name,string password)
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        var client = MagicOnionClient.Create<IUserService>(channel);
        try
        {//ìoò^ê¨å˜
            userId = await client.RegistUserAsync(name,password);
            return true;
        }catch(RpcException e)
        {//ìoò^ÇµÇ¡ÇœÇ¢
            Debug.Log(e);
            return false;
        }
    }
}
