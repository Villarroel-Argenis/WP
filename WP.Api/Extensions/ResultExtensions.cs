namespace WP.Api.Extensions;

/// <summary>
/// Extensiones para convertir Result a IResult de HTTP.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Convierte un Result en una respuesta HTTP apropiada.
    /// </summary>
    /// <typeparam name="T">Tipo del valor del resultado.</typeparam>
    /// <param name="result">El resultado a convertir.</param>
    /// <param name="onSuccess"></param>
    /// <returns>Una respuesta HTTP apropiada según el resultado.</returns>
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onSuccess) =>
        result.IsSuccess
            ? onSuccess(result.Value)
            : result.Error.Type switch
            {
                ErrorType.NotFound => Results.NotFound(new { result.Error.Description }),
                ErrorType.Validation => Results.BadRequest(new { result.Error.Description, errors= result.Error.Metadata }),
                ErrorType.Conflict => Results.Conflict(new { result.Error.Description }),
                _ => Results.StatusCode(500)
            };
}
