namespace WP.Application.Transactions.Events;


/// <summary>
/// Manejador del evento de dominio TransactionRegistered.
/// </summary>
public sealed partial class OnTransactionRegistered(ILogger<OnTransactionRegistered> logger)
    : IDomainEventConsumer<TransactionRegisteredDomainEvent>
{
    /// <summary>
    /// Maneja el evento de transacción registrada.
    /// </summary>
    /// <param name="domainEvent">El evento a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public Task HandleAsync(
        TransactionRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        LogTransacciónRegistradaTransactionidCuentaAccountidTipoType(domainEvent.TransactionId, domainEvent.AccountId, domainEvent.Type);

        return Task.CompletedTask;
    }

    [LoggerMessage(LogLevel.Information, "Transacción registrada: {TransactionId} | Cuenta: {AccountId} | Tipo: {Type}")]
    partial void LogTransacciónRegistradaTransactionidCuentaAccountidTipoType(Guid transactionId, Guid accountId, TransactionType type);
}
