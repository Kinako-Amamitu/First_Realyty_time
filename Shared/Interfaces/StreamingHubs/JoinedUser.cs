using System;
using System.Collections.Generic;
using System.Text;
using MasicOnionServer00.Model.Entity;
using MessagePack;
using UnityEngine;

namespace Shared.Interfaces.StreamingHubs
{
    [MessagePackObject]
    public class JoinedUser
    {
        [Key(0)]
        public Guid ConnectionId { get; set; } //接続ID
        [Key(1)]
        public User UserData {  get; set; }//ユーザー情報
        [Key(2)]
        public int JoinOrder { get; set; } //参加順番

        [Key(3)]
        public bool IsMaster {  get; set; } //マスタークライアントを判断
    }

}
