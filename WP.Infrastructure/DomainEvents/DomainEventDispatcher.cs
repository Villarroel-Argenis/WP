namespace WP.Infrastructure.DomainEvents;

/// <summary>
/// Implementación del despachador de eventos de dominio.
/// </summary>
public sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    /// <summary>
    /// Despacha todos los eventos de dominio de un agregado.
    /// </summary>
    /// <param name="aggregateRoot">El agregado que contiene los eventos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public async Task DispatchAsync(
        IAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        foreach (IDomainEvent domainEvent in aggregateRoot.DomainEvents)
        {
            Type handlerType = typeof(IDomainEventConsumer<>)
                .MakeGenericType(domainEvent.GetType());

            IEnumerable<object?> handlers = serviceProvider.GetServices(handlerType);

            foreach (object? handler in handlers)
            {
                await (Task)handlerType
                    .GetMethod(nameof(IDomainEventConsumer<IDomainEvent>.HandleAsync))!
                    .Invoke(handler, [domainEvent, cancellationToken])!;
            }
        }

        aggregateRoot.ClearDomainEvents();
    }
}
