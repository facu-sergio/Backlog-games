using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogGames.DataAccess.Layer.Models
{
    public class UserList : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserListGame> UserListGames { get; set; } = []; //navigation propertiy
    }
}
