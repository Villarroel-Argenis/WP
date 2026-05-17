namespace WP.Application.Transactions.RegisterTransaction;

/// <summary>
/// Manejador del comando para registrar una nueva transacción en una cuenta.
/// </summary>
/// <param name="accountRepository">Repositorio de cuentas.</param>
/// <param name="transactionRepository">Repositorio de transacciones.</param>
/// <param name="dispatcher">Disparador de domain events para agregados.</param>
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
        Account? account = await accountRepository.GetByIdAsync(command.AccountId, cancellationToken);
        if (account is null)
        {
            return AccountErrors.NotFound(command.AccountId);
        }

        Result<Currency> currencyResult = Currency.From(command.CurrencyCode);
        if (currencyResult.IsFailure)
        {
            return currencyResult.Error;
        }

        Result<List<Tag>> tagsResult = ParseTags(command.Tags);
        if (tagsResult.IsFailure)
        {
            return tagsResult.Error;
        }

        var amount = Money.Of(command.Amount, currencyResult.Value);

        Result<Transaction> transactionResult = command.Type switch
        {
            "Income" => HandleIncome(account, amount, command, tagsResult.Value),
            "Expense" => HandleExpense(account, amount, command, tagsResult.Value),
            "Transfer" => await HandleTransferAsync(account, amount, command, tagsResult.Value, cancellationToken),
            _ => TransactionErrors.TipoInvalido(command.Type)
        };

        if (transactionResult.IsFailure)
        {
            return transactionResult.Error;
        }

        Transaction transaction = transactionResult.Value;

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await accountRepository.UpdateAsync(account, cancellationToken);
        await dispatcher.DispatchAsync(transaction, cancellationToken);

        return transaction.Id;
    }

    /// <summary>
    /// Convierte los nombres de tags en objetos <see cref="Tag"/>.
    /// </summary>
    /// <param name="tags">Nombres de tags provenientes del comando.</param>
    /// <returns>Lista de tags o error de validación.</returns>
    private static Result<List<Tag>> ParseTags(IEnumerable<string>? tags)
    {
        if (tags is null)
        {
            return new List<Tag>();
        }

        var resultado = new List<Tag>();

        foreach (string nombre in tags)
        {
            Result<Tag> tagResult = Tag.From(nombre);
            if (tagResult.IsFailure)
            {
                return tagResult.Error;
            }

            resultado.Add(tagResult.Value);
        }

        return resultado;
    }

    /// <summary>
    /// Procesa una transacción de ingreso.
    /// </summary>
    private static Result<Transaction> HandleIncome(
        Account account,
        Money amount,
        RegisterTransactionCommand command,
        List<Tag> tags)
    {
        account.ApplyIncome(amount);
        return Transaction.CreateIncome(account.Id, amount, command.Description, tags);
    }

    /// <summary>
    /// Procesa una transacción de gasto.
    /// </summary>
    private static Result<Transaction> HandleExpense(
        Account account,
        Money amount,
        RegisterTransactionCommand command,
        List<Tag> tags)
    {
        account.ApplyExpense(amount);
        return Transaction.CreateExpense(account.Id, amount, command.Description, tags);
    }

    /// <summary>
    /// Procesa una transferencia entre cuentas.
    /// </summary>
    private async Task<Result<Transaction>> HandleTransferAsync(
        Account sourceAccount,
        Money amount,
        RegisterTransactionCommand command,
        List<Tag> tags,
        CancellationToken cancellationToken)
    {
        if (command.TargetAccountId is null)
        {
            return TransactionErrors.CuentaObjetivoRequerida();
        }

        Account? targetAccount = await accountRepository.GetByIdAsync(
            command.TargetAccountId.Value, cancellationToken);

        if (targetAccount is null)
        {
            return AccountErrors.NotFound(command.TargetAccountId.Value);
        }

        var transferId = Guid.NewGuid();

        sourceAccount.ApplyExpense(amount);
        targetAccount.ApplyIncome(amount);

        Result<Transaction> targetTransactionResult = Transaction.CreateTransfer(
            targetAccount.Id, amount, transferId, command.Description, tags);

        if (targetTransactionResult.IsFailure)
        {
            return targetTransactionResult.Error;
        }

        await transactionRepository.AddAsync(targetTransactionResult.Value, cancellationToken);
        await accountRepository.UpdateAsync(targetAccount, cancellationToken);

        return Transaction.CreateTransfer(
            sourceAccount.Id, amount, transferId, command.Description, tags);
    }
}
