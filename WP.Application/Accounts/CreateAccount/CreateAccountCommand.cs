namespace WP.Application.Accounts.CreateAccount;

/// <summary>
/// Representa el comando para crear una cuenta.
/// </summary>
/// <param name="Name">El nombre de la cuenta.</param>
/// <param name="InitialAmount">El monto inicial de la cuenta.</param>
/// <param name="CurrencyCode">El código de la moneda.</param>
public sealed record CreateAccountCommand(
    string Name
    ,decimal InitialAmount
    ,string CurrencyCode) : ICommand<Guid>;
