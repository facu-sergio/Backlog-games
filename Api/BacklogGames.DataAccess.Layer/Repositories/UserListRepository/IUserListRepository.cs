using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.Repositories.UserListRepository
{
    public interface IUserListRepository : IRepository<UserList>
    {
        Task UpdateNameAsync(int id, string name);
        Task<IEnumerable<Game>> GetGamesByListIdAsync(int listId);
    }
}
