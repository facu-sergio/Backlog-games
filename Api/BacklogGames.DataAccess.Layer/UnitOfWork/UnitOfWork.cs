using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Repositories;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGameRepository GameRepository { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            GameRepository = new GameRepository(context);
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
