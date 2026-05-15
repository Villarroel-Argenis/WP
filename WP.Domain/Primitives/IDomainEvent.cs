namespace WP.Domain.Primitives;

/// <summary>
/// Contrato base para todos los eventos de dominio.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Identificador único del evento.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Fecha en que ocurrió el evento.
    /// </summary>
    DateTime OccurredOn { get; }
}
