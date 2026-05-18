namespace WP.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Interceptor de EF Core que convierte los eventos de dominio en mensajes de outbox
/// antes de persistir los cambios, garantizando atomicidad entre datos y eventos.
/// </summary>
public sealed class OutboxInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepta el guardado de cambios de forma asíncrona para convertir
    /// los eventos de dominio pendientes en mensajes de outbox.
    /// </summary>
    /// <param name="eventData">Datos del evento de guardado.</param>
    /// <param name="result">Resultado interoperado del interceptor.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El resultado interoperado tras procesar los eventos.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            ConvertDomainEventsToOutboxMessages(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Extrae los eventos de dominio de todos los agregados rastreados
    /// y los convierte en mensajes de outbox.
    /// </summary>
    /// <param name="context">El contexto de base de datos.</param>
    private static void ConvertDomainEventsToOutboxMessages(DbContext context)
    {
        var outboxMessages = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .SelectMany(aggregate =>
            {
                var events = aggregate.DomainEvents.ToList();
                aggregate.ClearDomainEvents();
                return events;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
