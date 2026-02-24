using BacklogApp.Bussines.layer.Services;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.UserList;
using BacklogGames.Bussinnes.Layer.Services.UserListService;
using BacklogGames.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BacklogGames.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class UserListController : ControllerBase
    {
        private readonly IUserListService _userListService;
        public UserListController(IUserListService userListService)
        {
            _userListService = userListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllList()
        {
            var list = await _userListService.GetAllList();
            if (list == null || !list.Any())
            {
                return Ok(ResponseApiService.Response(404, null, "No se encontraron listas de juegos"));
            }
            return StatusCode(StatusCodes.Status200OK, ResponseApiService.Response(StatusCodes.Status200OK, list, "Lista de juegos obtenida correctamente"));
            
        }

        [HttpGet("{id}/with-games")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGamesByListId(int id)
        {
            var games = await _userListService.GetGamesByListIdAsync(id);

            if (games == null || !games.Any())
                return NotFound(ResponseApiService.Response(404, null, "No se encontraron juegos para la lista especificada."));

            return Ok(ResponseApiService.Response(200, games, "Juegos de la lista obtenidos correctamente"));
        }

        [HttpPost]
        public async Task<IActionResult> AddUserList([FromBody] string name)
        {
            var Userlist = await _userListService.AddUserList(name);
            return StatusCode(StatusCodes.Status201Created, Userlist);
        }

        [HttpPost("add-game-to-list/{listId:int}")]
        public async Task<IActionResult> AddGameToList([FromBody] AddGameToListDto dto)
        {
            var gameAdded = await _userListService.AddGameToListAsync(dto);
            return StatusCode(StatusCodes.Status201Created, gameAdded);
        }

        [HttpPatch("{listId:int}/games/{gameId:int}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGameStatus(int listId, int gameId, [FromBody] UpdateGameStatusDto dto)
        {
            var result = await _userListService.UpdateGameStatusAsync(listId, gameId, dto);
            return Ok(ResponseApiService.Response(200, result, "Estado del juego actualizado correctamente."));
        }

        [HttpGet("completed-games")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompletedGamesByYear([FromQuery] int year)
        {
            var games = await _userListService.GetCompletedGamesByYearAsync(year);
            if (games == null || !games.Any())
                return NotFound(ResponseApiService.Response(404, null, $"No se encontraron juegos completados en {year}."));

            return Ok(ResponseApiService.Response(200, games, $"Juegos completados en {year} obtenidos correctamente."));
        }
    }
}
