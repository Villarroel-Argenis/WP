namespace WP.Domain.Transactions.Events;

/// <summary>
/// Evento que se publica cuando se registra una nueva transacción.
/// </summary>
/// <param name="TransactionId">Identificador de la transacción registrada.</param>
/// <param name="AccountId">Identificador de la cuenta asociada.</param>
/// <param name="Type">Tipo de transacción registrada.</param>
public sealed record TransactionRegisteredDomainEvent(
    Guid TransactionId,
    Guid AccountId,
    TransactionType Type) : DomainEvent;
