using BacklogApp.Bussines.layer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BacklogGames.Exceptions
{
    public class ExceptionManager : IExceptionFilter
    {
        public void OnException (ExceptionContext context)
        {
            context.Result = new ObjectResult(ResponseApiService.Response(StatusCodes.Status500InternalServerError, null, context.Exception.Message));
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
