namespace BacklogGames.Bussinnes.Layer.Exceptions
{
    /// <summary>
    /// Excepción de dominio que incluye el código HTTP a devolver al cliente.
    /// Usar en lugar de excepciones genéricas para que el GlobalExceptionHandler
    /// pueda mapearla al status code correcto.
    /// </summary>
    public class CustomException : Exception
    {
        public int StatusCode { get; }

        public CustomException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
