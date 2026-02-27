using BacklogGames.DataAccess.Layer.Models;

namespace BacklogApp.DataAccess.Layer.Models
{
    public class Game : BaseEntity
    {
        public int IgdbId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? CoverUrl { get; set; }

        public long? FirstReleaseDate { get; set; }

        public string? Summary { get; set; }

        public double? Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int? HastilySeconds { get; set; }
        public int? NormallySeconds { get; set; }
        public int? CompletelySeconds { get; set; }
        public int? TimeToBeatCount { get; set; }

        public ICollection<UserListGame> UserListGames { get; set; } = []; //navigation propertiy

    }
}
