using BacklogGames.DataAccess.Layer.Repositories.GameRepository;
using BacklogGames.DataAccess.Layer.Repositories.UserListGameRepository;
using BacklogGames.DataAccess.Layer.Repositories.UserListRepository;

namespace BacklogGames.DataAccess.Layer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository GameRepository {  get; }
        IUserListRepository UserListRepository { get; }
        IUserListGameRepository UserListGameRepository { get; }
        Task SaveChangesAsync();

    }
}
