namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el manejador de comandos CreateAccount.
/// </summary>
public class CreateAccountCommandHandlerTests
{
    private readonly IAccountRepository _repository;
    private readonly CreateAccountCommandHandler _handler;
    private readonly IValidator<CreateAccountCommand> _validator;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public CreateAccountCommandHandlerTests()
    {
        _repository = Substitute.For<IAccountRepository>();
        _validator = Substitute.For<IValidator<CreateAccountCommand>>();
        _handler = new CreateAccountCommandHandler(_repository, _validator);
    }

    /// <summary>
    /// Verifica que el manejo con datos válidos retorna el ID de una cuenta nueva.
    /// </summary>
    [Fact]
    public async Task HandleConDatosValidosRetornaIdDeCuentaNuevaAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");

        Guid id = await _handler.Handle(command, CancellationToken.None);

        id.ShouldBeOfType<Guid>();
        id.ShouldNotBe(Guid.Empty);
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
    /// Verifica que el manejo con nombre vacío lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task HandleConNombreVacioLanzaArgumentExceptionAsync()
    {
        var command = new CreateAccountCommand("", 1_000m, "DOP");

        await Should.ThrowAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Verifica que el manejo con moneda inválida lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task HandleConMonedaInvalidaLanzaArgumentExceptionAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "XX");

        await Should.ThrowAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Verifica que el manejo con validación fallida lanza ValidationException.
    /// </summary>
    /// <exception cref="ValidationException"></exception>
    [Fact]
    public async Task HandleConValidacionFallidaLanzaValidationExceptionAsync()
    {
        var errors = new Dictionary<string, List<string>>
        {
            { "InitialAmount", ["El monto inicial no puede ser negativo."] }
        };

        _validator
            .When(v => v.Validate(Arg.Any<CreateAccountCommand>()))
            .Do(_ => throw new ValidationException(errors));

        var command = new CreateAccountCommand("Ahorros", -100m, "DOP");

        await Should.ThrowAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
