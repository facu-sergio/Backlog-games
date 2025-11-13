using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.Services.GameService;
using BacklogGames.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BacklogGames.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController( IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] CreateGameDto createGameDto)
        {
            var game = await _gameService.AddGame(createGameDto);
            return StatusCode(StatusCodes.Status201Created, game);
        }
    }
}
