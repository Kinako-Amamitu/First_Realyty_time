using Cysharp.Net.Http;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstModel : MonoBehaviour
{
    const string ServerURL = "http://localhost:7000";

    // Start is called before the first frame update
    void Start()
    {
        Sum(100, 323, result =>
        {
            Debug.Log(result);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Sum(int x, int y, Action<int> callback)
    {
        using var handler= new YetAnotherHttpHandler() { Http2Only = true };
        var channel= GrpcChannel.ForAddress(ServerURL,new GrpcChannelOptions() { HttpHandler=handler});
        var client=MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await client.SumAsync(x, y);
        callback?.Invoke(result);
    }
}
