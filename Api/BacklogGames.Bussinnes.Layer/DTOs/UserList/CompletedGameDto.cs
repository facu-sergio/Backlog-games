namespace BacklogGames.Bussinnes.Layer.DTOs.UserList
{
    public class CompletedGameDto
    {
        public int Id { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public double? Rating { get; set; }
        public int UserListId { get; set; }
        public string UserListName { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
