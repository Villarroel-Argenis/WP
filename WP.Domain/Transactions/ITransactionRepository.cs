namespace WP.Domain.Transactions;

/// <summary>
/// Define el contrato para el repositorio de transacciones.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Agrega una nueva transacción al repositorio.
    /// </summary>
    /// <param name="transaction">La transacción a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);


    /// <summary>
    /// Obtiene todas las transacciones de una cuenta.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de transacciones de la cuenta.</returns>
    Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken);
}
