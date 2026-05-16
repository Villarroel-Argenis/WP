namespace WP.Infrastructure.Persistence.Repositories;


/// <summary>
/// Implementación del repositorio de transacciones.
/// </summary>
/// /// <param name="context">Contexto de base de datos.</param>
public sealed class TransactionRepository(WpDbContext context) : ITransactionRepository
{
    /// <summary>
    /// Agrega una nueva transacción al repositorio.
    /// </summary>
    /// <param name="transaction">La transacción a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken) =>
        await context.Transactions.AddAsync(transaction, cancellationToken);

    /// <summary>
    /// Obtiene todas las transacciones de una cuenta.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de transacciones de la cuenta.</returns>
    public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        return await context.Transactions
            .Where(t => t.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }
}
