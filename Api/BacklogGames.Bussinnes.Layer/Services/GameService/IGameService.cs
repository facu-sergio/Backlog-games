using BacklogGames.Bussinnes.Layer.DTOs.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogGames.Bussinnes.Layer.Services.GameService
{
    public interface IGameService
    {
        Task<GameDto> AddGame(CreateGameDto game);
    }
}
