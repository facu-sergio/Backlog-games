using BacklogGames.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BacklogGames.DataAccess.Layer.Configurations
{
    public class GameStatusConfiguration : IEntityTypeConfiguration<GameStatus>
    {
        public void Configure(EntityTypeBuilder<GameStatus> entity)
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
        }
    }
}
