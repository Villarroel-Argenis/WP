namespace WP.Domain.Primitives;

/// <summary>
/// Contrato para los agregados raíz que pueden publicar eventos de dominio.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Obtiene los eventos de dominio pendientes de despachar.
    /// </summary>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Limpia los eventos de dominio después de ser despachados.
    /// </summary>
    void ClearDomainEvents();
}
