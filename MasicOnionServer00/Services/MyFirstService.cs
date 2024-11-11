using MagicOnion;
using MagicOnion.Server;
using Shared.Interfaces.Services;
namespace MasicOnionServer00.Services
{
    public class MyFirstService:ServiceBase<IMyFirstService>,IMyFirstService
    {

        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Console.WriteLine("Received"+x+", "+y);
            return x+y;
        }

        public async UnaryResult<int> SumAllAsync(int[] numList)
        {
            int sum = 0;
            for (int i = 0;i<numList.Length;i++)
            {
              sum += numList[i];
               
            }
            return sum;
        }

        public async UnaryResult<int[]> CalcForOperationAsync(int x, int y)
        {
            int[] result=new int[4];

            result[0] = x + y;
            result[1] = x-y;
            result[2] = x*y;
            result[3] = x/y;

            

                return result;
            
      
            
        }

        public async UnaryResult<float> SumAllNumberAsync(Number numArray)
        {
           return numArray.x+numArray.y;
        }
    }
}
