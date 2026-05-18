namespace WP.Infrastructure.Outbox;

/// <summary>
/// Servicio en background que procesa los mensajes pendientes del outbox,
/// deserializando y despachando los eventos de dominio correspondientes.
/// </summary>
/// <param name="serviceProvider">Proveedor de servicios para resolver dependencias por scope.</param>
/// <param name="logger">Logger para registrar el procesamiento.</param>
public sealed partial class OutboxProcessor(
    IServiceProvider serviceProvider,
    ILogger<OutboxProcessor> logger) : BackgroundService
{
    private static readonly TimeSpan _intervalo = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Ejecuta el procesador en background, consultando mensajes pendientes
    /// en intervalos regulares.
    /// </summary>
    /// <param name="stoppingToken">Token de cancelación para detener el servicio.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcesarMensajesPendientesAsync(stoppingToken);
            await Task.Delay(_intervalo, stoppingToken);
        }
    }

    /// <summary>
    /// Consulta y procesa todos los mensajes de outbox pendientes.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    private async Task ProcesarMensajesPendientesAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        WpDbContext dbContext = scope.ServiceProvider.GetRequiredService<WpDbContext>();
        IDomainEventDispatcher dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();

        List<OutboxMessage> mensajes = await dbContext.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        foreach (OutboxMessage mensaje in mensajes)
        {
            await ProcesarMensajeAsync(mensaje, dispatcher, dbContext, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Procesa un mensaje individual del outbox, deserializando y despachando el evento.
    /// </summary>
    /// <param name="mensaje">El mensaje a procesar.</param>
    /// <param name="dispatcher">El despachador de eventos de dominio.</param>
    /// <param name="dbContext">El contexto de base de datos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    private async Task ProcesarMensajeAsync(
        OutboxMessage mensaje,
        IDomainEventDispatcher dispatcher,
        WpDbContext dbContext,
        CancellationToken cancellationToken)
    {
        try
        {
            Type? tipo = System.Type.GetType(mensaje.Type);

            if (tipo is null)
            {
                LogTipoNoEncontrado(mensaje.Id, mensaje.Type);
                mensaje.ProcessedAt = DateTime.UtcNow;
                mensaje.Error = $"Tipo no encontrado: {mensaje.Type}";
                return;
            }

            IDomainEvent? domainEvent = JsonSerializer.Deserialize(mensaje.Content, tipo) as IDomainEvent;

            if (domainEvent is null)
            {
                LogDeserializacionFallida(mensaje.Id);
                mensaje.ProcessedAt = DateTime.UtcNow;
                mensaje.Error = "Error al deserializar el evento.";
                return;
            }

            await dispatcher.DispatchEventAsync(domainEvent, cancellationToken);

            mensaje.ProcessedAt = DateTime.UtcNow;

            LogMensajeProcesado(mensaje.Id, tipo.Name);
        }
        catch (Exception ex)
        {
            LogErrorProcesando(mensaje.Id, ex);
            mensaje.Error = ex.Message;
        }
    }

    [LoggerMessage(LogLevel.Warning, "Tipo no encontrado para el mensaje {MessageId}: {Type}")]
    partial void LogTipoNoEncontrado(Guid messageId, string type);

    [LoggerMessage(LogLevel.Warning, "Error al deserializar el mensaje {MessageId}")]
    partial void LogDeserializacionFallida(Guid messageId);

    [LoggerMessage(LogLevel.Information, "Mensaje {MessageId} procesado correctamente: {EventType}")]
    partial void LogMensajeProcesado(Guid messageId, string eventType);

    [LoggerMessage(LogLevel.Error, "Error procesando mensaje {MessageId}")]
    partial void LogErrorProcesando(Guid messageId, Exception ex);
}
