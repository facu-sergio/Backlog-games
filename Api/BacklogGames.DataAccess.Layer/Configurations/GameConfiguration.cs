using BacklogApp.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BacklogGames.DataAccess.Layer.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.IgdbId)
                  .IsUnique()
                  .HasDatabaseName("IX_Game_IgdbId");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.CoverUrl)
                .HasMaxLength(500);

            builder.Property(e => e.Summary)
                .HasMaxLength(2000);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
        }
    }
}
