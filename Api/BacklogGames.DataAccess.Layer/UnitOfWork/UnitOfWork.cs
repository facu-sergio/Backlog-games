using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Repositories.GameRepository;
using BacklogGames.DataAccess.Layer.Repositories.UserListGameRepository;
using BacklogGames.DataAccess.Layer.Repositories.UserListRepository;

namespace BacklogGames.DataAccess.Layer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGameRepository GameRepository { get; private set; }
        public IUserListRepository UserListRepository { get; private set; }
        public IUserListGameRepository UserListGameRepository { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            GameRepository = new GameRepository(context);
            UserListRepository = new UserListRepository(context);
            UserListGameRepository = new UserListGameRepository(context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
