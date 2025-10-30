using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogGames.Bussinnes.Layer.DTOs.Game
{
    public class GameDto
    {
        public int Id { get; set; }  // ✅ SÍ tiene Id
        public string Name { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public long? FirstReleaseDate { get; set; }
        public string? Summary { get; set; }
        public double? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
