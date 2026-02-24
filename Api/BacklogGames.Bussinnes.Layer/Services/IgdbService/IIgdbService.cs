using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.Igdb;

namespace BacklogGames.Bussinnes.Layer.Services.IgdbService
{
    public interface IIgdbService
    {
        Task<List<GameInfoDto>> SearchGamesByNameAsync(string searchTerm);
        Task<IgdbTimeToBeatResponseDto?> GetTimeToBeatAsync(int igdbGameId);
    }
}
