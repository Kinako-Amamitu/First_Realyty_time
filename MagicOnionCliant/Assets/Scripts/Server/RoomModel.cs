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

    //ユーザー退室通知
    public Action<JoinedUser> OnLeavedUser { get; set; }

    //位置回転同期
    public Action<JoinedUser,Vector3,Quaternion> OnMoveCharacter {  get; set; }

    //脱出通知
    public Action<JoinedUser> OnEscapeCharacter {  get; set; }

    //敵の出現処理
    public Action OnSpawnEnemy { get; set; }

    //敵の移動同期
    public Action<string,Vector3,Quaternion> OnMovedEnemy { get; set; }

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
    public async UniTask JoinedAsync(string roomName, int userId)
    {
        JoinedUser[] users = await roomHub.JoinedAsync(roomName, userId);
        foreach(var user in users)
        {
            if (user.UserData.Id == userId) this.ConnectionId = user.ConnectionId;
            OnJoinedUser(user);
        }
    }

    //入室通知(IRoomHubReceiverインターフェイスの実装)
    public void Onjoin(JoinedUser user)
    {
        OnJoinedUser(user);
    }

    //退室
    public async UniTask LeaveAsync()
    {
        await roomHub.LeavedAsync();
    }

    //退室通知
    public void OnLeave(JoinedUser user)
    {
        OnLeavedUser(user);
    }

    //移動
    public async Task MoveAsync(Vector3 pos,Quaternion rot)
    {
       await roomHub.MoveAsync(pos,rot);
    }

    //移動通知
    public void OnMove(JoinedUser user,Vector3 pos,Quaternion rot)
    {
        OnMoveCharacter(user, pos, rot);
    }


    public void OnSpawn()
    {
        OnSpawnEnemy();
    }

    public void OnMoveEnemy(string enemyName,Vector3 pos,Quaternion rot)
    {
        OnMovedEnemy(enemyName, pos,rot);
    }

    public async UniTask MoveEnemyAsync(string enemyName, Vector3 pos, Quaternion rot) 
    {
        await roomHub.EnemyMoveAsync(enemyName, pos, rot);
    }
}
