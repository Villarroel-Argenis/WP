using WP.Domain.Transactions.Events;

namespace WP.Domain.Transactions;

/// <summary>
/// Representa una transacción financiera en el sistema.
/// </summary>
public sealed class Transaction : IAggregateRoot
{
    private readonly List<Tag> _tags = [];
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>Identificador único de la transacción.</summary>
    public Guid Id { get; }

    /// <summary>Identificador de la cuenta asociada.</summary>
    public Guid AccountId { get; }

    /// <summary>Monto de la transacción.</summary>
    public Money Amount { get; }

    /// <summary>Tipo de transacción.</summary>
    public TransactionType Type { get; }

    /// <summary>Descripción opcional de la transacción.</summary>
    public string? Description { get; }

    /// <summary>Identificador de transferencia, solo aplica para transferencias.</summary>
    public Guid? TransferId { get; }

    /// <summary>Etiquetas asociadas a la transacción.</summary>
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

    /// <summary>Fecha de creación de la transacción.</summary>
    public DateTime CreatedAt { get; }

    private Transaction(
        Guid id,
        Guid accountId,
        Money amount,
        TransactionType type,
        string? description,
        Guid? transferId,
        List<Tag> tags,
        DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Description = description;
        TransferId = transferId;
        _tags = tags;
        CreatedAt = createdAt;
    }

    [ExcludeFromCodeCoverage]
#pragma warning disable CS8618
    private Transaction() { } // Para EF Core
#pragma warning restore CS8618

    /// <summary>
    /// Crea una transacción de ingreso.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="amount">Monto del ingreso.</param>
    /// <param name="description">Descripción opcional.</param>
    /// <param name="tags">Tags opcionales.</param>
    /// <returns>Un resultado con la transacción creada.</returns>
    public static Result<Transaction> CreateIncome(
        Guid accountId,
        Money amount,
        string? description = null,
        IEnumerable<Tag>? tags = null)
    {
        var transaction = new Transaction(
            Guid.NewGuid(),
            accountId,
            amount,
            TransactionType.Income,
            description,
            null,
            tags?.ToList() ?? [],
            DateTime.UtcNow);

        transaction.RaiseDomainEvent(new TransactionRegisteredDomainEvent(
            transaction.Id,
            transaction.AccountId,
            transaction.Type));

        return transaction;
    }

    /// <summary>
    /// Crea una transacción de gasto.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="amount">Monto del gasto.</param>
    /// <param name="description">Descripción opcional.</param>
    /// <param name="tags">Tags opcionales.</param>
    /// <returns>Un resultado con la transacción creada.</returns>
    public static Result<Transaction> CreateExpense(
        Guid accountId,
        Money amount,
        string? description = null,
        IEnumerable<Tag>? tags = null)
    {
        var transaction = new Transaction(
            Guid.NewGuid(),
            accountId,
            amount,
            TransactionType.Expense,
            description,
            null,
            tags?.ToList() ?? [],
            DateTime.UtcNow);

        transaction.RaiseDomainEvent(new TransactionRegisteredDomainEvent(
            transaction.Id,
            transaction.AccountId,
            transaction.Type));

        return transaction;
    }

    /// <summary>
    /// Crea una transacción de transferencia.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="amount">Monto de la transferencia.</param>
    /// <param name="transferId">Identificador de la transferencia.</param>
    /// <param name="description">Descripción opcional.</param>
    /// <param name="tags">Tags opcionales.</param>
    /// <returns>Un resultado con la transacción creada.</returns>
    public static Result<Transaction> CreateTransfer(
        Guid accountId,
        Money amount,
        Guid transferId,
        string? description = null,
        IEnumerable<Tag>? tags = null)
    {
        if (transferId == Guid.Empty)
            return TransactionErrors.TransferIdVacio();

        var transaction = new Transaction(
            Guid.NewGuid(),
            accountId,
            amount,
            TransactionType.Transfer,
            description,
            transferId,
            tags?.ToList() ?? [],
            DateTime.UtcNow);

        transaction.RaiseDomainEvent(new TransactionRegisteredDomainEvent(
            transaction.Id,
            transaction.AccountId,
            transaction.Type));

        return transaction;
    }

    /// <summary>Obtiene los eventos de dominio pendientes.</summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>Limpia los eventos de dominio después de ser despachados.</summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    private void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}
