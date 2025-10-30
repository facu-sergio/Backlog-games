using BacklogApp.DataAccess.Layer.Models;
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
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Game
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.IgdbId)
                      .IsUnique()
                      .HasDatabaseName("IX_Game_IgdbId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CoverUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Summary)
                    .HasMaxLength(2000);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false);

            });

            //Configuracion de la entidad UserList
            modelBuilder.Entity<UserList>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                .IsRequired();

                entity.Property(e => e.CreatedAt)
                        .HasDefaultValueSql("NOW()");

            });


            //configuracion de la entidad UserListGame
            modelBuilder.Entity<UserListGame>(entity =>
            {
                //clave compuesta
                entity.HasKey(ulg => new { ulg.GameId, ulg.UserListId });


                // Relación con Game
                entity.HasOne(ulg => ulg.Game)
                      .WithMany(e => e.UserListGames)
                      .HasForeignKey(ulg => ulg.GameId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con UserList
                entity.HasOne(ulg => ulg.UserList)
                      .WithMany(e => e.UserListGames)
                      .HasForeignKey(ulg => ulg.UserListId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con GameStatus
                entity.HasOne(ulg => ulg.GameStatus)
                      .WithMany(gs => gs.UserListGames)
                      .HasForeignKey(ulg => ulg.GameStatusId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ulg => ulg.GameStatusId)
                      .IsRequired()
                      .HasDefaultValue((int)GameProgressStatus.Pendiente);
            });

            // Configuración de la entidad GameStatus
            modelBuilder.Entity<GameStatus>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                // Seed data para los estados
                entity.HasData(
                    new GameStatus { Id = 1, Name = "Pendiente" },
                    new GameStatus { Id = 2, Name = "En Progreso" },
                    new GameStatus { Id = 3, Name = "Completado" }
                );
            });

        }
    }
}
