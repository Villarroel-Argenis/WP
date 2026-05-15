namespace WP.Application.Transactions.GetTransactionsByAccountId;

/// <summary>
/// Respuesta con los datos de una transacción.
/// </summary>
/// <param name="Id">Identificador único de la transacción.</param>
/// <param name="Amount">Monto de la transacción.</param>
/// <param name="CurrencyCode">Código de la moneda.</param>
/// <param name="Type">Tipo de transacción.</param>
/// <param name="Description">Descripción opcional.</param>
/// <param name="TransferId">Identificador de transferencia.</param>
/// <param name="Tags">Etiquetas de la transacción.</param>
/// <param name="CreatedAt">Fecha de creación.</param>
public sealed record TransactionResponse(
    Guid Id,
    decimal Amount,
    string CurrencyCode,
    string Type,
    string? Description,
    Guid? TransferId,
    IReadOnlyList<string> Tags,
    DateTime CreatedAt);
