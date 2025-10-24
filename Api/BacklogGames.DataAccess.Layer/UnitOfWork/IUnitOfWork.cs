using BacklogGames.DataAccess.Layer.Repositories;

namespace BacklogGames.DataAccess.Layer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository GameRepository {  get; }

        void SaveChanges();

    }
}
