namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para <see cref="LoggingBehavior{TCommand,TResult}"/>.
/// </summary>
public sealed class LoggingBehaviorTests
{
    private readonly LoggingBehavior<CreateAccountCommand, Guid> _behavior;

    /// <summary>
    /// Inicializa los mocks y el behavior bajo prueba.
    /// </summary>
    public LoggingBehaviorTests()
    {
        ILogger<LoggingBehavior<CreateAccountCommand, Guid>> logger = Substitute.For<ILogger<LoggingBehavior<CreateAccountCommand, Guid>>>();
        _behavior = new LoggingBehavior<CreateAccountCommand, Guid>(logger);
    }

    /// <summary>
    /// Debe invocar el siguiente handler en el pipeline.
    /// </summary>
    [Fact]
    public async Task HandleLlamaNextHandlerAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");
        bool nextCalled = false;

        await _behavior.Handle(command, () =>
        {
            nextCalled = true;
            return Task.FromResult(Result.Success(Guid.NewGuid()));
        });

        nextCalled.ShouldBeTrue();
    }

    /// <summary>
    /// Cuando el resultado es exitoso, debe retornarlo sin modificaciones.
    /// </summary>
    [Fact]
    public async Task HandleConResultadoExitosoRetornaResultadoAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");
        var expectedId = Guid.NewGuid();

        Result<Guid> result = await _behavior.Handle(command,
            () => Task.FromResult(Result.Success(expectedId)));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedId);
    }

    /// <summary>
    /// Cuando el resultado es fallido, debe retornarlo sin modificaciones.
    /// </summary>
    [Fact]
    public async Task HandleConResultadoFallidoRetornaErrorAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");
        Error error = AccountErrors.NotFound(Guid.NewGuid());

        Result<Guid> result = await _behavior.Handle(command,
            () => Task.FromResult(Result.Failure<Guid>(error)));

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(error);
    }
}
