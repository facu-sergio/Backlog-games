namespace BacklogGames.Bussinnes.Layer.DTOs.Game
{
    public class GameInfoDto
    {
        public int IgdbId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public long? FirstReleaseDate { get; set; }
        public string? Summary { get; set; }
        public double? Rating { get; set; }
    }
}
