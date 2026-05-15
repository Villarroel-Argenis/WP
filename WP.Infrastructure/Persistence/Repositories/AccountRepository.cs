namespace WP.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de cuentas utilizando Entity Framework.
/// </summary>
public sealed class AccountRepository(WpDbContext context) : IAccountRepository
{
    /// <summary>
    /// Agrega una cuenta de forma asíncrona.
    /// </summary>
    /// <param name="account">La cuenta a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Actualiza una cuenta existente en el repositorio.
    /// </summary>
    /// <param name="account">La cuenta a actualizar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Obtiene una cuenta por su identificador de forma asíncrona.
    /// </summary>
    /// <param name="id">El identificador único de la cuenta.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con la cuenta encontrada o null si no existe.</returns>
    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
}
