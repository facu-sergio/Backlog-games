using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.Repositories.GameRepository
{
    public interface IGameRepository : IRepository<Game>
    {
        Task UpdateAsync(Game game);
    }
}
