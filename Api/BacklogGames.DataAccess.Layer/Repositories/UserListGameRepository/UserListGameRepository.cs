using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;

namespace BacklogGames.DataAccess.Layer.Repositories.UserListGameRepository
{
    public class UserListGameRepository : Repository<UserListGame>, IUserListGameRepository
    {
        private readonly AppDbContext _context;

        public UserListGameRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task UpdateAsync(UserListGame game)
        {
            throw new NotImplementedException();
        }
    }
}
