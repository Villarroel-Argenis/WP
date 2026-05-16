namespace WP.Application.Transactions.RegisterTransaction;

/// <summary>
/// Manejador del comando para registrar una nueva transacción en una cuenta.
/// </summary>
/// <param name="accountRepository">Repositorio de cuentas.</param>
/// <param name="transactionRepository">Repositorio de transacciones.</param>
/// /// <param name="dispatcher">Disparador de domain events para agregados</param>
public sealed class RegisterTransactionCommandHandler(
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository,
    IDomainEventDispatcher dispatcher)
    : ICommandHandler<RegisterTransactionCommand, Guid>
{
    /// <summary>
    /// Maneja el comando de registro de transacción.
    /// </summary>
    /// <param name="command">El comando a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El identificador único de la transacción creada.</returns>
    public async Task<Result<Guid>> Handle(
        RegisterTransactionCommand command,
        CancellationToken cancellationToken)
    {

        Account account = await accountRepository.GetByIdAsync(
                              command.AccountId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Account), command.AccountId);

        var currency = Currency.From(command.CurrencyCode);
        var amount = Money.Of(command.Amount, currency);

        Transaction transaction = command.Type switch
        {
            "Income" => HandleIncome(account, amount, command),
            "Expense" => HandleExpense(account, amount, command),
            "Transfer" => await HandleTransferAsync(account, amount, command, cancellationToken),
            _ => throw new ArgumentException($"Tipo de transacción inválido: {command.Type}.")
        };

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await accountRepository.UpdateAsync(account, cancellationToken);

        await dispatcher.DispatchAsync(transaction, cancellationToken);

        return transaction.Id;
    }

    private static Transaction HandleIncome(
        Account account,
        Money amount,
        RegisterTransactionCommand command)
    {
        account.ApplyIncome(amount);
        return Transaction.CreateIncome(account.Id, amount, command.Description, command.Tags);
    }

    private static Transaction HandleExpense(
        Account account,
        Money amount,
        RegisterTransactionCommand command)
    {
        account.ApplyExpense(amount);
        return Transaction.CreateExpense(account.Id, amount, command.Description, command.Tags);
    }

    private async Task<Transaction> HandleTransferAsync(
        Account sourceAccount,
        Money amount,
        RegisterTransactionCommand command,
        CancellationToken cancellationToken)
    {
        if (command.TargetAccountId is null)
        {
            throw new ArgumentException("TargetAccountId es requerido para transferencias.");
        }

        Account targetAccount = await accountRepository.GetByIdAsync(
                                    command.TargetAccountId.Value, cancellationToken)
                                ?? throw new NotFoundException(nameof(Account), command.TargetAccountId.Value);

        var transferId = Guid.NewGuid();

        sourceAccount.ApplyExpense(amount);
        targetAccount.ApplyIncome(amount);

        var targetTransaction = Transaction.CreateTransfer(
            targetAccount.Id, amount, transferId, command.Description, command.Tags);

        await transactionRepository.AddAsync(targetTransaction, cancellationToken);
        await accountRepository.UpdateAsync(targetAccount, cancellationToken);

        return Transaction.CreateTransfer(
            sourceAccount.Id, amount, transferId, command.Description, command.Tags);
    }
}
