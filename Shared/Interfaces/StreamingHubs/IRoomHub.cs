using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub:IStreamingHub<IRoomHub,IRoomHubReceiver>
    {
        //ここにクライアント～サーバー定義

        //ユーザー入室
        Task<JoinedUser[]> JoinedAsync(string roomName, int userId);
    }
}
