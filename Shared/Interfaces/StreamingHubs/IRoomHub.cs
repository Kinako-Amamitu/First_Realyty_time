﻿using MagicOnion;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub:IStreamingHub<IRoomHub,IRoomHubReceiver>
    {
        //ここにクライアント～サーバー定義

        //ユーザー入室
        Task<JoinedUser[]> JoinedAsync(string roomName, int userId);

        //ユーザー退室
        Task LeavedAsync();

        //位置・回転をサーバーに送信
        Task MoveAsync(Vector3 pos, Quaternion rot);
    }
}
