using BacklogApp.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace BacklogGames.DataAccess.Layer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Game
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);

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
        }
    }
}
