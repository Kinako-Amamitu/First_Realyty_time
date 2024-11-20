using System;
using System.Collections.Generic;
using System.Text;
using MagicOnion;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReceiver
    {
        //ここにサーバー～クライアントの定義

        //ユーザーの入室通知
       void Onjoin(JoinedUser user);
    }
}
