namespace WP.Application.Accounts.GetAccountById;

/// <summary>
/// Manejador de la consulta GetAccountByIdQuery, responsable de procesar la solicitud de obtener una cuenta por su identificador y devolver la información correspondiente en un formato adecuado para la respuesta.
/// </summary>
/// <param name="repository"></param>
public sealed class GetAccountByIdQueryHandler(IAccountRepository repository)
    : IQueryHandler<GetAccountByIdQuery, AccountResponse?>
{
    /// <summary>
    /// Maneja la consulta para obtener una cuenta por su identificador. Busca la cuenta en el repositorio utilizando el ID proporcionado en la consulta. Si la cuenta existe, devuelve un objeto AccountResponse con los detalles de la cuenta; de lo contrario, devuelve null.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<AccountResponse?> Handle(
        GetAccountByIdQuery query,
        CancellationToken cancellationToken)
    {
        Account? account = await repository.GetByIdAsync(query.Id, cancellationToken);
        return account is null
            ? throw new NotFoundException(nameof(Account), query.Id)
            : new AccountResponse(
                account.Id,
                account.Name,
                account.Balance.Amount,
                account.Balance.Currency.Code,
                account.CreatedAt);
    }
}
