using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public class RoomHub : StreamingHubBase<IRoomHub,IRoomHubReceiver>,IRoomHub
    {
        private IGroup room;
        public async Task<JoinedUser[]>JoinAsync(string roomName, int userId)
        {
            //ルームに参加＆ルームを保持
            this.room = await this.Group.AddAsync(roomName);

            //DBからユーザー情報取得
            GameDbContext context = new GameDbContext();
            var user=context.Uses.Where(user=>user.Id==userId).First();

            //グループストレージにユーザーデータを格納
            var roomStorage=this.room.GetInMemoryStorage<RoomData>();
            var joinedUser = new JoinedUser() { ConnectionId = this.connctionId, UserData = userId };
            var roomData = new RoomData() { JoinedUser = joinedUser };
            roomStorage.Set(this.connectionId, roomData);

            //ルーム参加者全員に、ユーザーの入室通知を送信
            this.BroadcastExceptSelf(room).Onjoin(joinedUser);

            RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();
            //参加中のユーザー情報を返す
            JoinedUser[] joinedUserList = new JoinedUser[joinedUser.Length];

            return joinedUserList;
        }
    }
}
