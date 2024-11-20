using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoomModel : BaseModel, IRoomHubReceiver
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    //接続ID
    public Guid ConnectionId { get; set; }
    //ユーザー接続通知
    public Action<JoinedUser> OnJoinedUser { get; set; }

    //MagicOnion接続処理
    public async UniTask ConnectAsync()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress(ServerURL,
            new GrpcChannelOptions() { HttpHandler = handler });
        roomHub = await StreamingHubClient.ConnectAsync<IRoomHub, IRoomHubReceiver>(channel, this);
    }

    //MagicOnion切断処理
    public async UniTask DisconnectAsync()
    {
        if (roomHub != null) await roomHub.DisposeAsync();
        if (channel != null) await channel.ShutdownAsync();
        roomHub = null;channel = null;
    }

    //破棄処理
    async void OnDestroy()
    {
        DisconnectAsync();
    }

    //入室
    public async UniTask JoinAsync(string roomName, int userId)
    {
        JoinedUser[] users = await roomHub.JoinAsync(roomName, userId);
        foreach(var user in users)
        {
            if (user.UserData.id == userId) this.ConnectionId = user.ConnectionId;
            OnJoinedUser(user);
        }
    }

    //入室通知(IRoomHubReceiverインターフェイスの実装)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser(user);
    }
}
