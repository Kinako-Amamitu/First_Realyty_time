using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using MagicOnion;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReceiver
    {
        //ここにサーバー～クライアントの定義

        //ユーザーの入室通知
       void Onjoin(JoinedUser user);

        //ユーザーの退室通知
        void OnLeave(JoinedUser user);

        //プレイヤーの位置同期
        void OnMove(JoinedUser user,Vector3 pos,Quaternion rot);

        //敵のスポーン
        void OnSpawn();

        //敵の移動同期
        void OnMoveEnemy( string enemyName,Vector3 pos, Quaternion rot);

    }
}
