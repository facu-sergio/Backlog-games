using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace BacklogGames.DataAccess.Layer.Repositories.UserListRepository
{
    public class UserListRepository : Repository<UserList>, IUserListRepository
    {
        private readonly AppDbContext _context;
        public UserListRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserListGame>> GetGamesByListIdAsync(int listId)
        {
            return await _context.UserListGames
                .Include(x => x.Game)
                .Include(x => x.GameStatus)
                .Where(x => x.UserListId == listId)
                .ToListAsync();
        }

        public async Task UpdateNameAsync(int id, string name)
        {
            var list = await _context.UserList.FirstOrDefaultAsync(x => x.Id == id);
            list.Name = name;
            await _context.SaveChangesAsync();
        }
    }
}
