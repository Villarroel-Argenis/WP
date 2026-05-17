namespace WP.Application.Accounts.CreateAccount;

/// <summary>
/// Manejador para el comando CreateAccount.
/// </summary>
/// <param name="accountRepository">El repositorio de cuentas.</param>
public sealed class CreateAccountCommandHandler(
    IAccountRepository accountRepository) : ICommandHandler<CreateAccountCommand, Guid>
{
    /// <summary>
    /// Maneja el comando para crear una cuenta.
    /// </summary>
    /// <param name="command">El comando a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>El identificador único de la cuenta creada.</returns>
    public async Task<Result<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        Result<Currency> currency = Currency.From(command.CurrencyCode);
        if (currency.IsFailure)
        {
            return currency.Error;
        }

        var balance = Money.Of(command.InitialAmount, currency.Value);
        var account = Account.Create(command.Name, balance);

        await accountRepository.AddAsync(account, cancellationToken);

        return account.Id;
    }
}
