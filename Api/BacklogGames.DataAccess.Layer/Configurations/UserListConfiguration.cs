using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BacklogGames.DataAccess.Layer.Configurations
{
    public class UserListConfiguration : IEntityTypeConfiguration<UserList>
    {
        public void Configure(EntityTypeBuilder<UserList> entity)
        {
            //Configuracion de la entidad UserList
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
            .IsRequired();

            entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()");
        }
    }
}
