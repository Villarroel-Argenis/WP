namespace WP.Application.Transactions.GetTransactionsByAccountId;


/// <summary>
/// Manejador de la consulta para obtener transacciones de una cuenta.
/// </summary>
/// <param name="accountRepository">Repositorio de cuentas.</param>
/// <param name="transactionRepository">Repositorio de transacciones.</param>
public sealed class GetTransactionsByAccountIdQueryHandler(
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository)
    : IQueryHandler<GetTransactionsByAccountIdQuery, IReadOnlyList<TransactionResponse>>
{
    /// <summary>
    /// Maneja la consulta de transacciones por cuenta.
    /// </summary>
    /// <param name="query">La consulta a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de transacciones de la cuenta.</returns>
    public async Task<IReadOnlyList<TransactionResponse>> Handle(
        GetTransactionsByAccountIdQuery query,
        CancellationToken cancellationToken)
    {
        _ = await accountRepository.GetByIdAsync(
                              query.AccountId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Account), query.AccountId);

        IReadOnlyList<Transaction> transactions = await transactionRepository.GetByAccountIdAsync(
            query.AccountId, cancellationToken);

        return transactions.Select(t => new TransactionResponse(
            t.Id,
            t.Amount.Amount,
            t.Amount.Currency.Code,
            t.Type.ToString(),
            t.Description,
            t.TransferId,
            t.Tags.Select(tag => tag.Name).ToList(),
            t.CreatedAt)).ToList();
    }
}
