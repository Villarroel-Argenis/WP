namespace WP.Application.Abstractions;


/// <summary>
/// Contrato para el despachador de eventos de dominio.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Despacha todos los eventos de dominio de un agregado.
    /// </summary>
    /// <param name="aggregateRoot">El agregado que contiene los eventos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task DispatchAsync(IAggregateRoot aggregateRoot, CancellationToken cancellationToken);
}
