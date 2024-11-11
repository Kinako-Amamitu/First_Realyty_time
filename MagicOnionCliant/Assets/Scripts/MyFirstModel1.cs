using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstModel1 : MonoBehaviour
{
    const string ServerURL = "http://localhost:7000";

    // Start is called before the first frame update
    async void Start()
    {
        int result = await Sum(100, 323);
        Debug.Log(result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async UniTask<int> Sum(int x, int y)
    {
        var handler= new YetAnotherHttpHandler() { Http2Only = true };
        var channel= GrpcChannel.ForAddress(ServerURL,new GrpcChannelOptions() { HttpHandler=handler});
        var client=MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.SumAsync(x, y);
        return result;
    }
}
