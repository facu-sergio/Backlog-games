using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.Data;
using BacklogGames.DataAccess.Layer.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace BacklogGames.DataAccess.Layer.Repositories.GameRepository
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Game game)
        {
            var obj = await _context.Games.FirstOrDefaultAsync(g => g.Id == game.Id);
            obj.Name = game.Name;
            await _context.SaveChangesAsync();
        }

    }
}
