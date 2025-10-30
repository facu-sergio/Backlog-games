namespace BacklogGames.DataAccess.Layer.Models
{
    public class GameStatus : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? ColorHex { get; set; }

        public string? Description { get; set; }
        public ICollection<UserListGame> UserListGames { get; set; } = new List<UserListGame>();
    }
}
