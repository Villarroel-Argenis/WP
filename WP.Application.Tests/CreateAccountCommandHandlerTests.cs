namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el manejador de comandos CreateAccount.
/// </summary>
public class CreateAccountCommandHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly CreateAccountCommandHandler _handler;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public CreateAccountCommandHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _handler = new CreateAccountCommandHandler(_repository);
    }

    /// <summary>
    /// Verifica que el manejo con datos válidos retorna el ID de una cuenta nueva.
    /// </summary>
    [Fact]
    public async Task HandleConDatosValidosRetornaIdDeCuentaNuevaAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");

        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);
    }

    /// <summary>
    /// Verifica que el manejo con datos válidos llama al repositorio una vez.
    /// </summary>
    [Fact]
    public async Task HandleConDatosValidosLlamaRepositorioUnaVezAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");

        await _handler.Handle(command, CancellationToken.None);

        await _repository.Received(1).AddAsync(
            Arg.Any<Account>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifica que el manejo con moneda inválida retorna error de validación.
    /// </summary>
    [Fact]
    public async Task HandleConMonedaInvalidaRetornaErrorAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "XX");

        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Currency.CodigoInvalido");
    }

    /// <summary>
    /// Verifica que el manejo con moneda inválida no llama al repositorio.
    /// </summary>
    [Fact]
    public async Task HandleConMonedaInvalidaNoLlamaRepositorioAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "XX");

        await _handler.Handle(command, CancellationToken.None);

        await _repository.DidNotReceive().AddAsync(
            Arg.Any<Account>(),
            Arg.Any<CancellationToken>());
    }
}
