namespace WP.Domain.Outbox;

/// <summary>
/// Representa un mensaje pendiente de procesamiento en el patrón Outbox.
/// </summary>
public sealed class OutboxMessage
{
    /// <summary>
    /// Identificador único del mensaje.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Nombre completo del tipo del evento de dominio serializado.
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Contenido serializado del evento de dominio en formato JSON.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora en que fue creado el mensaje.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Fecha y hora en que fue procesado el mensaje. Null si aún no fue procesado.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Error ocurrido durante el procesamiento. Null si no hubo error.
    /// </summary>
    public string? Error { get; set; }
}
