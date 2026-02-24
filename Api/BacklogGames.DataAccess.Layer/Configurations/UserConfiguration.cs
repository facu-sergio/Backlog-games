using BacklogGames.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BacklogGames.DataAccess.Layer.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(e => e.Username)
                .IsUnique();

            builder.Property(e => e.PasswordHash)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.HasData(new User
            {
                Id = 1,
                Username = "facu",
                PasswordHash = "$2a$12$FNyKv/SF4zxlBbEnVpdkd.xfpMiCPHJmyghsxCA7/VAtsUQ2ICcUy",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }
    }
}
