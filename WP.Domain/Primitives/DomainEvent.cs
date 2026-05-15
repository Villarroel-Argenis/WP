namespace WP.Domain.Primitives;

/// <summary>
/// Implementación base para los eventos de dominio.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// Identificador único del evento.
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    /// Fecha en que ocurrió el evento.
    /// </summary>
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
