using System.Text.Json.Serialization;

namespace BacklogGames.Bussinnes.Layer.DTOs.Igdb
{
    public class IgdbGameResponseDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("cover")]
        public IgdbCoverDto? Cover { get; set; }

        [JsonPropertyName("first_release_date")]
        public long? FirstReleaseDate { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("rating")]
        public double? Rating { get; set; }
    }

    public class IgdbCoverDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("image_id")]
        public string ImageId { get; set; } = string.Empty;
    }
}
