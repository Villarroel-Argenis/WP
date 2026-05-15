namespace WP.Application.Accounts.GetAccountById;

/// <summary>
/// Consulta para obtener una cuenta por su identificador.
/// </summary>
/// <param name="Id">El identificador único de la cuenta que se desea obtener.</param>
public sealed record GetAccountByIdQuery(Guid Id) : IQuery<AccountResponse?>;
