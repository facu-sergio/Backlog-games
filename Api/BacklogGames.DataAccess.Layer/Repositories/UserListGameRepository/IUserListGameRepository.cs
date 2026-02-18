using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.Repositories.UserListGameRepository
{
    public interface IUserListGameRepository : IRepository<UserListGame>
    {
        Task UpdateAsync(UserListGame game);
        Task UpdateGameStatusAsync(int gameId, int listId, int statusId, DateTime? completedAt);
        Task<IEnumerable<UserListGame>> GetCompletedByYearAsync(int year);
    }
}
