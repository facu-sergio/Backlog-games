using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.Services.UserListService;
using BacklogGames.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BacklogGames.Controllers
{
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
    }
}
