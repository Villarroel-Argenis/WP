namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para <see cref="UnitOfWorkBehavior{TCommand,TResult}"/>.
/// </summary>
public class UnitOfWorkBehaviorTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UnitOfWorkBehavior<CreateAccountCommand, Guid> _behavior;

    /// <summary>
    /// Inicializa los mocks y el behavior bajo prueba.
    /// </summary>
    public UnitOfWorkBehaviorTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _behavior = new UnitOfWorkBehavior<CreateAccountCommand, Guid>(_unitOfWork);
    }

    /// <summary>
    /// Cuando el resultado es exitoso, debe persistir los cambios.
    /// </summary>
    [Fact]
    public async Task HandleConResultadoExitosoLlamaSaveChangesAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");
        var expectedId = Guid.NewGuid();

        Result<Guid> result = await _behavior.Handle(command,
            () => Task.FromResult(Result.Success(expectedId)));

        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Cuando el resultado es fallido, no debe persistir los cambios.
    /// </summary>
    [Fact]
    public async Task HandleConResultadoFallidoNoLlamaSaveChangesAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");

        Result<Guid> result = await _behavior.Handle(command,
            () => Task.FromResult(Result.Failure<Guid>(AccountErrors.NotFound(Guid.NewGuid()))));

        result.IsFailure.ShouldBeTrue();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// El resultado del handler debe devolverse sin modificaciones.
    /// </summary>
    [Fact]
    public async Task HandleRetornaElResultadoDelHandlerSinModificacionesAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");
        var expectedId = Guid.NewGuid();

        Result<Guid> result = await _behavior.Handle(command,
            () => Task.FromResult(Result.Success(expectedId)));

        result.Value.ShouldBe(expectedId);
    }
}
