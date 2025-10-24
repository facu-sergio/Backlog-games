using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        // Métodos específicos para Game (puedes agregar más según necesites)
        Task UpdateAsync(Game game);
    }
}
