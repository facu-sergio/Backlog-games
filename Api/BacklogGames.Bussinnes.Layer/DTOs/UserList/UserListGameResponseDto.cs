namespace BacklogGames.Bussinnes.Layer.DTOs.UserList
{
    public class UserListGameResponseDto
    {
        public int GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public int UserListId { get; set; }
        public string UserListName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
    }
}
