using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.Services.IgdbService;
using BacklogGames.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BacklogGames.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class GamesController : ControllerBase
    {
        private readonly IIgdbService _igdbService;

        public GamesController(IIgdbService igdbService)
        {
            _igdbService = igdbService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<GameInfoDto>>> SearchGames([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                return BadRequest("Search term must be at least 3 characters long.");
            }

            var games = await _igdbService.SearchGamesByNameAsync(name);
            return Ok(games);
        }
    }
}
