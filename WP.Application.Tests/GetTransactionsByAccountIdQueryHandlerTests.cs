using WP.Application.Transactions.GetTransactionsByAccountId;

namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el manejador de consulta de transacciones por cuenta.
/// </summary>
public sealed class GetTransactionsByAccountIdQueryHandlerTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly GetTransactionsByAccountIdQueryHandler _handler;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public GetTransactionsByAccountIdQueryHandlerTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _handler = new GetTransactionsByAccountIdQueryHandler(
            _accountRepository,
            _transactionRepository);
    }

    /// <summary>
    /// Verifica que consultar transacciones de una cuenta existente retorna la lista correcta.
    /// </summary>
    [Fact]
    public async Task HandleConCuentaExistenteRetornaTransaccionesAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));
        var transactions = new List<Transaction>
        {
            Transaction.CreateIncome(account.Id, Money.Of(500m, Currency.Dop), "Salario", ["salario"]),
            Transaction.CreateExpense(account.Id, Money.Of(200m, Currency.Dop), "Supermercado")
        };

        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);
        _transactionRepository.GetByAccountIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(transactions);

        Result<IReadOnlyList<TransactionResponse>> result = await _handler.Handle(
            new GetTransactionsByAccountIdQuery(account.Id),
            CancellationToken.None);

        result.Value.Count.ShouldBe(2);
        result.Value[0].Id.ShouldNotBe(Guid.Empty);
        result.Value[1].Id.ShouldNotBe(Guid.Empty);
        result.Value[0].Type.ShouldBe("Income");
        result.Value[0].Amount.ShouldBe(500m);
        result.Value[0].CurrencyCode.ShouldBe("DOP");
        result.Value[0].Description.ShouldBe("Salario");
        result.Value[0].Tags.ShouldContain("salario");
        result.Value[1].Type.ShouldBe("Expense");
        result.Value[1].Amount.ShouldBe(200m);
        result.Value[1].Description.ShouldBe("Supermercado");
    }

    /// <summary>
    /// Verifica que consultar transacciones de una cuenta sin transacciones retorna lista vacía.
    /// </summary>
    [Fact]
    public async Task HandleConCuentaSinTransaccionesRetornaListaVaciaAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));

        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);
        _transactionRepository.GetByAccountIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(new List<Transaction>());

        Result<IReadOnlyList<TransactionResponse>> result = await _handler.Handle(
            new GetTransactionsByAccountIdQuery(account.Id),
            CancellationToken.None);

        result.Value.ShouldBeEmpty();
    }

    /// <summary>
    /// Verifica que consultar transacciones de una cuenta inexistente lanza NotFoundException.
    /// </summary>
    [Fact]
    public async Task HandleConCuentaInexistenteLanzaNotFoundExceptionAsync()
    {
        _accountRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        await Should.ThrowAsync<NotFoundException>(() =>
            _handler.Handle(
                new GetTransactionsByAccountIdQuery(Guid.NewGuid()),
                CancellationToken.None));
    }

    /// <summary>
    /// Verifica que el response de una transferencia incluye el TransferId.
    /// </summary>
    [Fact]
    public async Task HandleConTransferenciaRetornaTransferIdAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));
        var transferId = Guid.NewGuid();
        var transactions = new List<Transaction>
        {
            Transaction.CreateTransfer(account.Id, Money.Of(300m, Currency.Dop), transferId)
        };

        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);
        _transactionRepository.GetByAccountIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(transactions);

        Result<IReadOnlyList<TransactionResponse>> result = await _handler.Handle(
            new GetTransactionsByAccountIdQuery(account.Id),
            CancellationToken.None);

        result.Value[0].TransferId.ShouldBe(transferId);
        result.Value[0].Type.ShouldBe("Transfer");
    }
}
