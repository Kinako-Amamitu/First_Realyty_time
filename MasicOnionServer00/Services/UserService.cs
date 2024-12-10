using MagicOnion;
using MagicOnion.Server;
using MasicOnionServer00.Model.Context;
using MasicOnionServer00.Model.Entity;
using Shared.Interfaces.Services;

namespace MasicOnionServer00.Services
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        public async UnaryResult<int> RegistUserAsync(string name, string password)
        {
            using var context = new GameDbContext();

            //バリデーションチェック
            if (context.Users.Where(user => user.Name == name).Count() > 0)
            {
                throw new ReturnStatusException(Grpc.Core.StatusCode.InvalidArgument, "");
            }

            //テーブルにレコードを追加
            User user=new User();
            user.Name = name;
            user.Password = password;
            user.Token = "";
            user.Created_at = DateTime.Now;
            user.Updated_at = DateTime.Now;
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.Id;
            
        }
    }
}
