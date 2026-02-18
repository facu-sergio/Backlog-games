using BacklogGames.DataAccess.Layer.Enums;
using BacklogGames.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BacklogGames.DataAccess.Layer.Configurations
{
    public class UserListGameConfiguration : IEntityTypeConfiguration<UserListGame>
    {
        public void Configure(EntityTypeBuilder<UserListGame> entity)
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

            entity.Property(ulg => ulg.CompletedAt)
                  .IsRequired(false);
        }
    }
}
