namespace WP.Application.Abstractions;

/// <summary>
/// Define el contrato para la unidad de trabajo, que coordina la persistencia de cambios.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste todos los cambios pendientes en el almacenamiento de datos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>El número de entradas escritas en la base de datos.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
