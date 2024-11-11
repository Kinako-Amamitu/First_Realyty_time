using MagicOnion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    public interface IMyFirstService:IService<IMyFirstService>
    {
        //足し算
        UnaryResult<int>SumAsync(int x,int y);
    }
}
