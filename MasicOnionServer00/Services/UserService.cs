using MagicOnion;
using MagicOnion.Server;
using MasicOnionServer00.Model.Context;
using MasicOnionServer00.Model.Entity;
using Shared.Interfaces.Services;

namespace MasicOnionServer00.Services
{
    public class UserService : ServiceBase<IUserService>,IUserService
    {
        public async UnaryResult<int> ResistUserAsync(string name)
        {
            using var context = new GameDbContextcs();

            //テーブルにレコードを追加
            User user=new User();
            user.Name = name;
            user.Token = "";
            user.Created_at = DateTime.Now;
            user.Updated_at = DateTime.Now;
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.id;
        }
    }
}
