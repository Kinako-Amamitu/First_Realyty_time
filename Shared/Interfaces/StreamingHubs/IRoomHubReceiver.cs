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

        //プレイヤーの位置・回転・アニメーション同期
        void OnMove(JoinedUser user,Vector3 pos,Quaternion rot,int anim);

        //敵のスポーン
        void OnSpawn(string enemyName, Vector3 pos);

        //敵の移動同期
        void OnMoveEnemy( string enemyName,Vector3 pos, Quaternion rot);

        //マスタークライアント譲渡処理
        void OnMasterClient(JoinedUser joinedUser);

        //オブジェクトの生成同期
        void OnObjectSpawn(string objectName, Vector3 pos);

        //オブジェクトの移動回転同期
        void OnObjectMove(string objectName, Vector3 pos, Quaternion rot);
    }
}
