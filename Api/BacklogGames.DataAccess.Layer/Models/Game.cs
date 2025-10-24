namespace BacklogApp.DataAccess.Layer.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? CoverUrl { get; set; }

        public long? FirstReleaseDate { get; set; }

        public string? Summary { get; set; }

        public double? Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

    }
}
