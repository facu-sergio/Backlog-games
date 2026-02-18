using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Enums;
using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

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

        public async Task UpdateGameStatusAsync(int gameId, int listId, int statusId, DateTime? completedAt)
        {
            var entry = await _context.UserListGames
                .FirstOrDefaultAsync(ulg => ulg.GameId == gameId && ulg.UserListId == listId)
                ?? throw new Exception($"No se encontró el juego {gameId} en la lista {listId}.");

            entry.GameStatusId = statusId;
            entry.CompletedAt = statusId == (int)GameProgressStatus.Terminado
                ? completedAt?.ToUniversalTime() ?? DateTime.UtcNow
                : null;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserListGame>> GetCompletedByYearAsync(int year)
        {
            return await _context.UserListGames
                .Include(ulg => ulg.Game)
                .Include(ulg => ulg.UserList)
                .Where(ulg => ulg.GameStatusId == (int)GameProgressStatus.Terminado
                           && ulg.CompletedAt.HasValue
                           && ulg.CompletedAt.Value.Year == year)
                .ToListAsync();
        }
    }
}
