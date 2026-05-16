namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el manejador de consultas GetAccountById.
/// </summary>
public class GetAccountByIdQueryHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly GetAccountByIdQueryHandler _handler;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public GetAccountByIdQueryHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _handler = new GetAccountByIdQueryHandler(_repository);
    }

    /// <summary>
    /// Verifica que el manejo con un ID de cuenta existente retorna la cuenta correcta.
    /// </summary>
    [Fact]
    public async Task HandleConIdExistenteRetornaCuentaCorrectaAsync()
    {
        var account = Account.Create("Ahorros", Money.Of(1000, Currency.Dop));

        _repository.GetByIdAsync(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        Result<AccountResponse?> response = await _handler.Handle(new GetAccountByIdQuery(account.Id), CancellationToken.None);

        response.ShouldNotBeNull();
        response.Value!.Name.ShouldBe("Ahorros");
        response.Value.Id.ShouldBe(account.Id);
        response.Value.Amount.ShouldBe(1000);
        response.Value.CurrencyCode.ShouldBe("DOP");
    }

    /// <summary>
    /// Verifica que el manejo con un ID de cuenta inexistente lanza NotFoundException.
    /// </summary>
    [Fact]
    public async Task HandleConIdInexistenteLanzaNotoFoundExceptionAsync()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Account?)null);

        await Should.ThrowAsync<NotFoundException>(() => _handler.Handle(new GetAccountByIdQuery(id), CancellationToken.None));
    }
}
