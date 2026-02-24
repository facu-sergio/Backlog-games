using System.Text.Json.Serialization;

namespace BacklogGames.Bussinnes.Layer.DTOs.Igdb
{
    public class IgdbTimeToBeatResponseDto
    {
        [JsonPropertyName("game_id")]
        public int GameId { get; set; }

        [JsonPropertyName("hastily")]
        public int? Hastily { get; set; }

        [JsonPropertyName("normally")]
        public int? Normally { get; set; }

        [JsonPropertyName("completely")]
        public int? Completely { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }
    }
}
