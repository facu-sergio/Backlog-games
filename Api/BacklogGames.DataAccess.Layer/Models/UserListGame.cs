using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Enums;

namespace BacklogGames.DataAccess.Layer.Models
{
    public class UserListGame
    {
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int UserListId { get; set; }
        public UserList UserList { get; set; }

        public int GameStatusId { get; set; } = (int)GameStatusEnum.Pendiente;
        public GameStatus GameStatus { get; set; } = null!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

    }
}
