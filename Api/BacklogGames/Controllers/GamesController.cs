using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BacklogGames.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class GamesController : ControllerBase
    {
       
        [HttpGet]
        public IActionResult GetAll()
        {
            //var response = _gameService.GetAllGames();
            return StatusCode(StatusCodes.Status200OK, "hola");
        }

       
    }
}
