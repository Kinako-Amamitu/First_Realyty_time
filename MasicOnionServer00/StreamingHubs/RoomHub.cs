using MagicOnion.Server.Hubs;
using MasicOnionServer00.Model.Context;
using MasicOnionServer00.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace StreamingHubs
{
    public class RoomHub : StreamingHubBase<IRoomHub, IRoomHubReceiver>, IRoomHub
    {
        private IGroup room;
        public async Task<JoinedUser[]>JoinedAsync(string roomName, int userId)
        {
            //ルームに参加＆ルームを保持
            this.room = await this.Group.AddAsync(roomName);

            //DBからユーザー情報取得
            GameDbContext context = new GameDbContext();
            var user=context.Users.Where(user=>user.Id==userId).First();

            //グループストレージにユーザーデータを格納
            var roomStorage=this.room.GetInMemoryStorage<RoomData>();
            var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId, UserData = user};
            var roomData = new RoomData() { JoinedUser = joinedUser };
            roomStorage.Set(this.ConnectionId, roomData);

            //ルーム参加者全員に、ユーザーの入室通知を送信
            this.BroadcastExceptSelf(room).Onjoin(joinedUser);

            RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();
            //参加中のユーザー情報を返す
            JoinedUser[] joinedUserList = new JoinedUser[roomDataList.Length];
            for (int i = 0; i < roomDataList.Length; i++)
            {
                joinedUserList[i] = roomDataList[i].JoinedUser;
            }
          

            return joinedUserList;
        }

        public async Task LeavedAsync()
        {
            //グループデータから削除
            this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

            var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId };

            //ルーム参加者全員に、ユーザーの退室通知を送信
            this.Broadcast(room).OnLeave(joinedUser);

            //ルーム内のメンバーから自分を削除
            await room.RemoveAsync(this.Context);

        }

        public async Task MoveAsync(Vector3 pos,Quaternion rot)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);
            //位置と回転を保存
            roomData.Poslot = new Poslot() { Pos = pos, Lot = rot };

            //参加ユーザーを取得
            var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId };

            //ルーム参加者全員に、ユーザーの移動回転を送信
            this.BroadcastExceptSelf(room).OnMove(joinedUser,pos,rot);

        }

        protected override ValueTask OnDisconnected()
        {
            if (this.room != null) 
            {
                //ルームデータを削除
                this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

                var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId };

                //退室したことを全メンバーに通知
                this.Broadcast(room).OnLeave(joinedUser);
                //ルーム内のメンバーから削除
                room.RemoveAsync(this.Context);

                
            }
            return CompletedTask;
        }
    }
}
