namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el behavior de logging del pipeline.
/// </summary>
public sealed class LoggingBehaviorTests
{
    private readonly ILogger<LoggingBehavior<CreateAccountCommand, Guid>> _logger;
    private readonly LoggingBehavior<CreateAccountCommand, Guid> _behavior;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public LoggingBehaviorTests()
    {
        _logger = Substitute.For<ILogger<LoggingBehavior<CreateAccountCommand, Guid>>>();
        _behavior = new LoggingBehavior<CreateAccountCommand, Guid>(_logger);
    }

    /// <summary>
    /// Verifica que el behavior llama al siguiente paso del pipeline.
    /// </summary>
    [Fact]
    public async Task HandleLlamaNextHandlerAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");
        bool nextCalled = false;

        await _behavior.Handle(
            command,
            () =>
            {
                nextCalled = true;
                return Task.FromResult(Guid.NewGuid());
            },
            CancellationToken.None);

        nextCalled.ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que el behavior retorna el resultado del siguiente paso.
    /// </summary>
    [Fact]
    public async Task HandleRetornaResultadoDeNextHandlerAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");
        var expectedId = Guid.NewGuid();

        Guid result = await _behavior.Handle(
            command,
            () => Task.FromResult(expectedId),
            CancellationToken.None);

        result.ShouldBe(expectedId);
    }
}
