namespace WP.Api.Middleware;

/// <summary>
/// Manejador global de excepciones que captura y maneja las excepciones no controladas en la aplicación, proporcionando respuestas adecuadas según el tipo de excepción.
/// </summary>
/// <param name="logger"></param>
public partial class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Intenta manejar una excepción capturada durante el procesamiento de una solicitud HTTP, determinando el código de estado y el mensaje de error apropiados según el tipo de excepción, y escribiendo una respuesta JSON con los detalles del error.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Solicitud invalida"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno dek servidor")
        };

        LogExcepcionCapturadaMessage(exception.Message, exception);
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    [LoggerMessage(LogLevel.Error, "Excepcion capturada: {Message}")]
    partial void LogExcepcionCapturadaMessage(string message, Exception exception);
}
