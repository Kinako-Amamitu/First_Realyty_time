using MagicOnion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    public interface IUserService: IService<IUserService>
    {
        /// <summary>
        /// ユーザー登録を行う
        /// </summary>
        /// <param name="name">ユーザーの名前</param>
        /// <returns></returns>
        UnaryResult<int> ResistUserAsync(string name);
    }

}
