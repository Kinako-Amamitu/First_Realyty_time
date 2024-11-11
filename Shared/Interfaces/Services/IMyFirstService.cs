using MagicOnion;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    public interface IMyFirstService:IService<IMyFirstService>
    {
        /// <summary>
        /// 足し算処理を行う
        /// </summary>
        /// <param name="x">足す数</param>
        /// <param name="y">足される数</param>
        /// <returns>xとyの合計値</returns>
        UnaryResult<int>SumAsync(int x,int y);

        //受け取った配列の値の合計を返す
        UnaryResult<int> SumAllAsync(int[] numList);

        //x+yを[0]に、x-yを[1]に、x*yを[2]に、x/yを[3]に入れて配列で返す
        UnaryResult<int[]> CalcForOperationAsync(int x, int y);

        //xとyの少数をフィールドに持つNumberクラスを渡して、x+yの結果を返す
        UnaryResult<float> SumAllNumberAsync(Number numArray);
    }

    [MessagePackObject]
    public class Number
    {
        [Key(0)]
        public float x;
        [Key(1)]
        public float y;
    }
}
