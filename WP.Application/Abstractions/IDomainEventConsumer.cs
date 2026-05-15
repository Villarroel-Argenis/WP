namespace WP.Application.Abstractions;


/// <summary>
/// Contrato para los manejadores de eventos de dominio.
/// </summary>
/// <typeparam name="TDomainEvent">Tipo de evento de dominio a manejar.</typeparam>
public interface IDomainEventConsumer<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// Maneja el evento de dominio.
    /// </summary>
    /// <param name="domainEvent">El evento a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
