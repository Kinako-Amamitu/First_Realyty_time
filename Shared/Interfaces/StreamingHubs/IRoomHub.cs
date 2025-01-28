using MagicOnion;
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

        //位置・回転・アニメーションをサーバーに送信
        Task MoveAsync(Vector3 pos, Quaternion rot,int anim);

        //敵の出現処理
        Task SpawnAsync(string enemyName, Vector3 pos);

        //敵の位置回転
        Task EnemyMoveAsync(string enemyName,Vector3 pos,Quaternion rot);

        //マスタークライアントが退室したときの処理
        Task MasterLostAsync();


        //オブジェクトの生成同期
        Task ObjectSpawnAsync(Guid connectionId,string objectName,Vector3 pos,Quaternion rot);

        //オブジェクトの位置回転同期
        Task ObjectMoveAsync(string objectName,Vector3 pos,Quaternion rot);
    }
}
