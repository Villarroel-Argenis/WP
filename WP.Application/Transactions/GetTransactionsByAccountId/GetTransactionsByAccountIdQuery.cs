namespace WP.Application.Transactions.GetTransactionsByAccountId;

/// <summary>
/// Consulta para obtener las transacciones de una cuenta.
/// </summary>
/// <param name="AccountId">Identificador de la cuenta.</param>
public sealed record GetTransactionsByAccountIdQuery(Guid AccountId) : IQuery<IReadOnlyList<TransactionResponse>>;
