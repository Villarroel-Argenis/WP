namespace WP.Application.Abstractions;

/// <summary>
/// Define un contrato para los manejadores de consultas que devuelven un resultado.
/// </summary>
/// <typeparam name="TQuery">El tipo de la consulta a manejar.</typeparam>
/// <typeparam name="TResult">El tipo del resultado que devuelve el manejador.</typeparam>
[SuppressMessage(
    "Major Code Smell", "S2326:Unused type parameters should be removed",
    Justification = "TResult es un marcador que obliga al handler a retornar el tipo correcto.")]
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Maneja de forma asíncrona la consulta especificada y devuelve un resultado.
    /// </summary>
    /// <param name="query">La consulta a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el resultado.</returns>
    Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
