namespace WP.Application.Accounts.CreateAccount;

/// <summary>
/// Manejador para el comando CreateAccount.
/// </summary>
/// <param name="accountRepository">El repositorio de cuentas.</param>
/// <param name="validator">El validador para el comando CreateAccountCommand.</param>
public sealed class CreateAccountCommandHandler(IAccountRepository accountRepository, IValidator<CreateAccountCommand> validator) : ICommandHandler<CreateAccountCommand, Guid>
{
    /// <summary>
    /// Maneja el comando para crear una cuenta.
    /// </summary>
    /// <param name="command">El comando a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>El identificador único de la cuenta creada.</returns>
    public async Task<Guid> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        validator.Validate(command);
        var currency = Currency.From(command.CurrencyCode);
        var balance = Money.Of(command.InitialAmount, currency);
        var account = Account.Create(command.Name, balance);

        await accountRepository.AddAsync(account, cancellationToken);

        return account.Id;
    }
}
