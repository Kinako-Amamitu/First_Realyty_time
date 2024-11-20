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
    public async UniTask JoinAsync(string roomName, int userId)
    {
        JoinedUser[] users = await roomHub.JoinAsync(roomName, userId);
        foreach(var user in users)
        {
            if (user.UserData.id == userId) this.ConnectionId = user.ConnectionId;
            OnJoinedUser(user);
        }
    }

    //�����ʒm(IRoomHubReceiver�C���^�[�t�F�C�X�̎���)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser(user);
    }
}
