﻿using MagicOnion.Server.Hubs;
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

        //入室
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
            //1人目をマスタークライアントにする
            if (roomStorage.AllValues.Count==0) { joinedUser.IsMaster = true; }

            //ルーム参加者全員に、ユーザーの入室通知を送信
            this.BroadcastExceptSelf(room).Onjoin(joinedUser);

            var roomData = new RoomData() { JoinedUser = joinedUser };
            roomStorage.Set(this.ConnectionId, roomData);

          

            RoomData[] roomDataList = roomStorage.AllValues.ToArray<RoomData>();

            //参加中のユーザー情報を返す
            JoinedUser[] joinedUserList = new JoinedUser[roomDataList.Length];
            
            for (int i = 0; i < roomDataList.Length; i++)
            {
                joinedUserList[i] = roomDataList[i].JoinedUser;
            }

            
        


            
          

            return joinedUserList;
        }

        //退室
        public async Task LeavedAsync()
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //マスタークライアントだったら次の人に譲渡する
            if(roomData.JoinedUser.IsMaster==true)
            {
                await MasterLostAsync();
            }

            //グループデータから削除
            this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

            var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId };

            //if (joinedUser.IsMaster == true) {await OnMasterClient(); }

            //ルーム参加者全員に、ユーザーの退室通知を送信
            this.Broadcast(room).OnLeave(joinedUser);



            //ルーム内のメンバーから自分を削除
            await room.RemoveAsync(this.Context);

        }

        //マスタークライアント譲渡
        public async Task MasterLostAsync()
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();

            //すべてのルームデータ取得
            RoomData [] roomDataList = roomStorage.AllValues.ToArray<RoomData>();

            //参加中のユーザー情報を返す
            JoinedUser[] joinedUserList = new JoinedUser[roomDataList.Length];

                for (int i = 0; i<roomDataList.Length; i++)
                {
                    joinedUserList[i] = roomDataList[i].JoinedUser;

                    if (joinedUserList[i].IsMaster!=true) 
                    {
                        joinedUserList[i].IsMaster = true;

                        JoinedUser newMaster = joinedUserList[i];
                        //ルーム参加者全員に、ユーザーのマスタークライアント化を送信
                        this.Broadcast(room).OnMasterClient(newMaster);
                    break;
                    }
                }

            
        }

        ////マスタークライアント譲渡
        //public async Task OnMasterClient()
        //{
        //    //グループストレージからRoomData取得
        //    var roomStorage = this.room.GetInMemoryStorage<RoomData>();
        //    var roomData = roomStorage.Get(this.ConnectionId);

        //    roomData.JoinedUser.ConnectionId

        //    //参加ユーザーを取得
        //    //var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId };
        //}

        //ユーザーの移動回転
        public async Task MoveAsync(Vector3 pos,Quaternion rot, int anim)
        {
            //グループストレージからRoomData取得
            var roomStorage =this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);
            //位置と回転を保存
            roomData.Position = pos;
            roomData.Rotation = rot;

            //参加ユーザーを取得
            var joinedUser = new JoinedUser() { ConnectionId = this.ConnectionId };

            //ルーム参加者全員に、ユーザーの移動回転を送信
            this.BroadcastExceptSelf(room).OnMove(joinedUser,pos,rot,anim);

        }

        //敵の生成処理
        public async Task SpawnAsync(string enemyName, Vector3 pos)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //位置を保存
            roomData.Position = pos;

            //ルーム参加者全員に、敵の出現を送信
            this.Broadcast(room).OnSpawn(enemyName, pos);

        }

        //敵のIDを同期する
        public async Task EnemyIdAsync(int enemyId)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //ルーム参加者全員に、敵の出現を送信
            this.BroadcastExceptSelf(room).OnIdEnemy(enemyId);
        }

        //敵の移動回転処理
        public async Task EnemyMoveAsync(string enemyName, Vector3 pos,Quaternion rot)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //位置と回転を保存
            roomData.Position = pos;
            roomData.Rotation = rot;

            //ルーム参加者全員に、ユーザーの移動回転を送信
            this.BroadcastExceptSelf(room).OnMoveEnemy(enemyName,pos,rot);
        }

        //敵の撃破同期
        public async Task EnemyExcusionAsync(string enemyName)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //ルーム参加者全員に、ユーザーの移動回転を送信
            this.BroadcastExceptSelf(room).OnExcusionEnemy(enemyName);
        }

        //オブジェクトの生成同期
        public async Task ObjectSpawnAsync(Guid connectionId,string objectName,Vector3 pos, Quaternion rot, Vector3 fow)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //位置を保存
            roomData.Position = pos;
            roomData.Rotation= rot;
            roomData.Forword = fow;

            //ルーム参加者全員に、オブジェクトの生成を送信
            this.Broadcast(room).OnObjectSpawn(connectionId,objectName, pos,rot,fow);
        }

        //オブジェクトの移動回転同期
        public async Task ObjectMoveAsync(string objectName,Vector3 pos,Quaternion rot)
        {
            //グループストレージからRoomData取得
            var roomStorage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStorage.Get(this.ConnectionId);

            //位置と回転を保存
            roomData.Position = pos;
            roomData.Rotation = rot;

            //ルーム参加者全員に、ユーザーの移動回転を送信
            this.BroadcastExceptSelf(room).OnObjectMove(objectName, pos, rot);
        }

        //切断時の処理
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
