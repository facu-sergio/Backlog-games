using BacklogGames.Bussinnes.Layer.DTOs.Game;

namespace BacklogGames.Bussinnes.Layer.Services.IgdbService
{
    public interface IIgdbService
    {
        Task<List<GameInfoDto>> SearchGamesByNameAsync(string searchTerm);
    }
}
