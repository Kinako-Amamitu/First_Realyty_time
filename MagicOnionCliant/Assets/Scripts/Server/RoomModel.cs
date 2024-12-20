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

    //�ڑ�ID
    public Guid ConnectionId { get; set; }
    //���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser { get; set; }

    //���[�U�[�ގ��ʒm
    public Action<JoinedUser> OnLeavedUser { get; set; }

    //�ʒu��]����
    public Action<JoinedUser,Vector3,Quaternion> OnMoveCharacter {  get; set; }

    //�E�o�ʒm
    public Action<JoinedUser> OnEscapeCharacter {  get; set; }

    //�G�̏o������
    public Action OnSpawnEnemy { get; set; }

    //�G�̈ړ�����
    public Action<string,Vector3,Quaternion> OnMovedEnemy { get; set; }

    //MagicOnion�ڑ�����
    public async UniTask ConnectAsync()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress(ServerURL,
            new GrpcChannelOptions() { HttpHandler = handler });
        roomHub = await StreamingHubClient.ConnectAsync<IRoomHub, IRoomHubReceiver>(channel, this);
    }

    //MagicOnion�ؒf����
    public async UniTask DisconnectAsync()
    {
        if (roomHub != null) await roomHub.DisposeAsync();
        if (channel != null) await channel.ShutdownAsync();
        roomHub = null;channel = null;
    }

    //�j������
    async void OnDestroy()
    {
        DisconnectAsync();
    }

    //����
    public async UniTask JoinedAsync(string roomName, int userId)
    {
        JoinedUser[] users = await roomHub.JoinedAsync(roomName, userId);
        foreach(var user in users)
        {
            if (user.UserData.Id == userId) this.ConnectionId = user.ConnectionId;
            OnJoinedUser(user);
        }
    }

    //�����ʒm(IRoomHubReceiver�C���^�[�t�F�C�X�̎���)
    public void Onjoin(JoinedUser user)
    {
        OnJoinedUser(user);
    }

    //�ގ�
    public async UniTask LeaveAsync()
    {
        await roomHub.LeavedAsync();
    }

    //�ގ��ʒm
    public void OnLeave(JoinedUser user)
    {
        OnLeavedUser(user);
    }

    //�ړ�
    public async Task MoveAsync(Vector3 pos,Quaternion rot)
    {
       await roomHub.MoveAsync(pos,rot);
    }

    //�ړ��ʒm
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
