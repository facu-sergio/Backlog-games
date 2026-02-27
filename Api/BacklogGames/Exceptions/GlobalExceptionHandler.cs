using BacklogApp.Bussines.layer.Services;
using BacklogGames.Bussinnes.Layer.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BacklogGames.Exceptions
{
    /// <summary>
    /// Manejador global de excepciones. Intercepta todas las excepciones no controladas,
    /// las loggea y devuelve una respuesta JSON uniforme usando ResponseApiService.
    /// Las CustomException se mapean a su status code correspondiente;
    /// cualquier otra excepción devuelve 500.
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception: {Message}", exception.Message);

            int statusCode;
            string message;

            if (exception is CustomException customEx)
            {
                statusCode = customEx.StatusCode;
                message = customEx.Message;
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Ocurrió un error inesperado.";
            }

            context.Response.StatusCode = statusCode;

            var response = ResponseApiService.Response(statusCode, null, message);
            await context.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
