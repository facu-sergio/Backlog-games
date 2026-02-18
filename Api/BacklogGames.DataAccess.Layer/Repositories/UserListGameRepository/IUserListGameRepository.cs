using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.Repositories.UserListGameRepository
{
    public interface IUserListGameRepository : IRepository<UserListGame>
    {
        Task UpdateAsync(UserListGame game);
        Task MarkAsCompletedAsync(int gameId, int listId, DateTime completedAt);
        Task<IEnumerable<UserListGame>> GetCompletedByYearAsync(int year);
    }
}
