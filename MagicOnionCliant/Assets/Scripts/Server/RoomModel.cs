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
    public bool IsMaster { get; set; }

    //�ڑ�ID
    public Guid ConnectionId { get; set; }
    //���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser { get; set; }

    //���[�U�[�ގ��ʒm
    public Action<JoinedUser> OnLeavedUser { get; set; }

    //�ʒu��]����
    public Action<JoinedUser,Vector3,Quaternion,int> OnMoveCharacter {  get; set; }

    //�E�o�ʒm
    public Action<JoinedUser> OnEscapeCharacter {  get; set; }

    //�G�̏o������
    public Action<string,Vector3> OnSpawnEnemy { get; set; }

    //�G�̈ړ�����
    public Action<string,Vector3,Quaternion> OnMovedEnemy { get; set; }

    public Action<JoinedUser> OnMasteredClient { get; set; }

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
            if (user.UserData.Id == userId) 
                this.ConnectionId = user.ConnectionId;
                this.IsMaster=user.IsMaster;
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

    public void OnMasterClient(JoinedUser user)
    {
        OnMasteredClient(user);
    }

    //�ړ�
    public async Task MoveAsync(Vector3 pos,Quaternion rot,int anim)
    {
       await roomHub.MoveAsync(pos,rot,anim);
    }

    //�ړ��ʒm
    public void OnMove(JoinedUser user,Vector3 pos,Quaternion rot,int anim)
    {
        OnMoveCharacter(user, pos, rot,anim);
    }

    //�G�̏o���ʒm
    public void OnSpawn(string enemyName, Vector3 pos)
    {
        OnSpawnEnemy(enemyName,pos);
    }

    //�G�̏o��
    public async UniTask SpawnEnemyAsync(string enemyName,Vector3 pos)
    {
        await roomHub.SpawnAsync(enemyName,pos);
    }

    //�G�̈ړ���]
    public void OnMoveEnemy(string enemyName,Vector3 pos,Quaternion rot)
    {
        OnMovedEnemy(enemyName, pos,rot);
    }

    public async UniTask MoveEnemyAsync(string enemyName, Vector3 pos, Quaternion rot) 
    {
        await roomHub.EnemyMoveAsync(enemyName, pos, rot);
    }

    public async UniTask MasterLostAsync()
    {
        await roomHub.MasterLostAsync();
    }
}
