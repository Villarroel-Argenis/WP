namespace WP.Api.Middleware;

/// <summary>
/// Manejador global de excepciones que captura excepciones no controladas,
/// proporcionando respuestas adecuadas según el tipo de excepción.
/// </summary>
/// <param name="logger">Logger para registrar las excepciones capturadas.</param>
public partial class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Intenta manejar una excepción no controlada durante el procesamiento de una solicitud HTTP.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP de la solicitud.</param>
    /// <param name="exception">La excepción capturada.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Siempre retorna <c>true</c> indicando que la excepción fue manejada.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int statusCode, string title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Errores de validación"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        LogExcepcionCapturadaMessage(exception.Message, exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToArray());
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    [LoggerMessage(LogLevel.Error, "Excepcion capturada: {Message}")]
    partial void LogExcepcionCapturadaMessage(string message, Exception exception);
}
