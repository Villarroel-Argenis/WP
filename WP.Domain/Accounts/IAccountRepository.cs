namespace WP.Domain.Accounts;

/// <summary>
/// Define las operaciones para el repositorio de cuentas.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Agrega una cuenta de forma asíncrona.
    /// </summary>
    /// <param name="account">La cuenta a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task AddAsync(Account account, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una cuenta por su identificador de forma asíncrona.
    /// </summary>
    /// <param name="id">El identificador único de la cuenta.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con la cuenta encontrada o null si no existe.</returns>
    Task<Account?> GetByIdAsync(Guid id,  CancellationToken cancellationToken = default);
}
