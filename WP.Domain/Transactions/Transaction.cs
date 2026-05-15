namespace WP.Domain.Transactions;

/// <summary>
/// Representa una transacción financiera en el sistema.
/// </summary>
public sealed class Transaction
{
    private readonly List<Tag> _tags = [];

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

    /// <summary>Tags asociados a la transacción.</summary>
    /// <summary>
    /// Etiquetas asociadas a la transacción.
    /// </summary>
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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Transaction() { } // Para EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Crea una transacción de ingreso.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="amount">Monto del ingreso.</param>
    /// <param name="description">Descripción opcional.</param>
    /// <param name="tags">Tags opcionales.</param>
    /// <returns>Una nueva transacción de ingreso.</returns>
    public static Transaction CreateIncome(
        Guid accountId,
        Money amount,
        string? description = null,
        IEnumerable<string>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(amount);

        return new Transaction(
            Guid.NewGuid(),
            accountId,
            amount,
            TransactionType.Income,
            description,
            null,
            tags?.Select(Tag.From).ToList() ?? [],
            DateTime.UtcNow);
    }

    /// <summary>
    /// Crea una transacción de gasto.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="amount">Monto del gasto.</param>
    /// <param name="description">Descripción opcional.</param>
    /// <param name="tags">Tags opcionales.</param>
    /// <returns>Una nueva transacción de gasto.</returns>
    public static Transaction CreateExpense(
        Guid accountId,
        Money amount,
        string? description = null,
        IEnumerable<string>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(amount);

        return new Transaction(
            Guid.NewGuid(),
            accountId,
            amount,
            TransactionType.Expense,
            description,
            null,
            tags?.Select(Tag.From).ToList() ?? [],
            DateTime.UtcNow);
    }

    /// <summary>
    /// Crea una transacción de transferencia.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="amount">Monto de la transferencia.</param>
    /// <param name="transferId">Identificador de la transferencia.</param>
    /// <param name="description">Descripción opcional.</param>
    /// <param name="tags">Tags opcionales.</param>
    /// <returns>Una nueva transacción de transferencia.</returns>
    public static Transaction CreateTransfer(
        Guid accountId,
        Money amount,
        Guid transferId,
        string? description = null,
        IEnumerable<string>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(amount);

        if (transferId == Guid.Empty)
        {
            throw new ArgumentException("TransferId no puede ser vacío.", nameof(transferId));
        }

        return new Transaction(
            Guid.NewGuid(),
            accountId,
            amount,
            TransactionType.Transfer,
            description,
            transferId,
            tags?.Select(Tag.From).ToList() ?? [],
            DateTime.UtcNow);
    }
}
