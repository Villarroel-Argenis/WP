
namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el manejador de registro de transacciones.
/// </summary>
public sealed class RegisterTransactionCommandHandlerTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly RegisterTransactionCommandHandler _handler;
    private readonly IDomainEventDispatcher _dispatcher;
    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public RegisterTransactionCommandHandlerTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        ITransactionRepository transactionRepository = Substitute.For<ITransactionRepository>();
        _dispatcher = Substitute.For<IDomainEventDispatcher>();
        _handler = new RegisterTransactionCommandHandler(
            _accountRepository,
            transactionRepository,
            _dispatcher);
    }

    /// <summary>
    /// Verifica que registrar un ingreso válido retorna el id de la transacción.
    /// </summary>
    [Fact]
    public async Task HandleConIngresoValidoRetornaIdDeTransaccionAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));
        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new RegisterTransactionCommand(
            account.Id, 500m, "DOP", "Income", "Salario");

        Result<Guid> id = await _handler.Handle(command, CancellationToken.None);

        id.Value.ShouldNotBe(Guid.Empty);
        account.Balance.Amount.ShouldBe(1_500m);
    }

    /// <summary>
    /// Verifica que registrar un gasto válido reduce el balance de la cuenta.
    /// </summary>
    [Fact]
    public async Task HandleConGastoValidoReduceElBalanceAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));
        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new RegisterTransactionCommand(
            account.Id, 300m, "DOP", "Expense", "Supermercado");

        await _handler.Handle(command, CancellationToken.None);

        account.Balance.Amount.ShouldBe(700m);
    }

    /// <summary>
    /// Verifica que registrar una transferencia actualiza ambas cuentas.
    /// </summary>
    [Fact]
    public async Task HandleConTransferenciaValidaActualizaAmbasCuentasAsync()
    {
        var sourceAccount = Account.Create("Cuenta Origen", Money.Of(1_000m, Currency.Dop));
        var targetAccount = Account.Create("Cuenta Destino", Money.Of(500m, Currency.Dop));

        _accountRepository.GetByIdAsync(sourceAccount.Id, Arg.Any<CancellationToken>())
            .Returns(sourceAccount);
        _accountRepository.GetByIdAsync(targetAccount.Id, Arg.Any<CancellationToken>())
            .Returns(targetAccount);

        var command = new RegisterTransactionCommand(
            sourceAccount.Id, 200m, "DOP", "Transfer",
            TargetAccountId: targetAccount.Id);

        await _handler.Handle(command, CancellationToken.None);

        sourceAccount.Balance.Amount.ShouldBe(800m);
        targetAccount.Balance.Amount.ShouldBe(700m);
    }

    /// <summary>
    /// Verifica que registrar una transacción con cuenta inexistente lanza NotFoundException.
    /// </summary>
    [Fact]
    public async Task HandleConCuentaInexistenteLanzaNotFoundExceptionAsync()
    {
        _accountRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 500m, "DOP", "Income");

        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Account.NotFound");
    }

    /// <summary>
    /// Verifica que registrar un tipo de transacción inválido lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task HandleConTipoInvalidoLanzaArgumentExceptionAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));
        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new RegisterTransactionCommand(
            account.Id, 500m, "DOP", "Invalid");

        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Transaction.TipoInvalido");
        result.Error.Description.ShouldBe("El tipo de transacción 'Invalid' no es válido.");
    }

    /// <summary>
    /// Verifica que registrar un ingreso despacha el evento de dominio.
    /// </summary>
    [Fact]
    public async Task HandleConIngresoValidoDespachaEventoDeDominioAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));
        _accountRepository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new RegisterTransactionCommand(
            account.Id, 500m, "DOP", "Income", "Salario");

        await _handler.Handle(command, CancellationToken.None);

        await _dispatcher.Received(1).DispatchAsync(
            Arg.Any<IAggregateRoot>(),
            Arg.Any<CancellationToken>());
    }
}
