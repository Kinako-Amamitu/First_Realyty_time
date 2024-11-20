using MasicOnionServer00.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace MasicOnionServer00.Model.Context
{
    public class GameDbContext : DbContext
    {
        public DbSet<User>Users { get; set; }

        readonly string connectionString =
            "server=localhost;database=realtime_game;user=jobi;password=jobi";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString,new MySqlServerVersion(new Version(8, 0,0)));
        }
    }
}
