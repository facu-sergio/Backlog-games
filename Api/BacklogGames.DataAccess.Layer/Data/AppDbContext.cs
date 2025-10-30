using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Configurations;
using BacklogGames.DataAccess.Layer.Enums;
using BacklogGames.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace BacklogGames.DataAccess.Layer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<UserList> UserList { get; set; }
        public DbSet<UserListGame> UserListGames { get; set; }
        public DbSet<GameStatus> GameStatus { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new UserListConfiguration());
            modelBuilder.ApplyConfiguration(new UserListGameConfiguration());
            modelBuilder.ApplyConfiguration(new GameStatusConfiguration());
            base.OnModelCreating(modelBuilder); 
        }
    }
}
