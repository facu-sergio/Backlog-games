namespace BacklogGames.Bussinnes.Layer.DTOs.Game
{
    public class AddGameToListDto
    {
        public int UserListId { get; set; }
        public GameInfoDto GameInfo { get; set; } = null!;
        public int? StatusId { get; set; }
    }
}
