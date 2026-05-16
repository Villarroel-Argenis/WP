namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para <see cref="ValidationBehavior{TCommand,TResult}"/>.
/// </summary>
public sealed class ValidationBehaviorTests
{
    private readonly IValidator<CreateAccountCommand> _validator;
    private readonly ValidationBehavior<CreateAccountCommand, Guid> _behavior;

    /// <summary>
    /// Inicializa los mocks y el behavior bajo prueba.
    /// </summary>
    public ValidationBehaviorTests()
    {
        _validator = Substitute.For<IValidator<CreateAccountCommand>>();
        _behavior = new ValidationBehavior<CreateAccountCommand, Guid>(_validator);
    }

    /// <summary>
    /// Cuando la validación es exitosa, el siguiente handler debe ser invocado.
    /// </summary>
    [Fact]
    public async Task HandleConValidacionExitosaLlamaNextHandlerAsync()
    {
        var command = new CreateAccountCommand("Mi Cuenta", 1_000m, "DOP");
        var expectedId = Guid.NewGuid();
        bool nextCalled = false;

        Result<Guid> result = await _behavior.Handle(command, () =>
        {
            nextCalled = true;
            return Task.FromResult(Result.Success(expectedId));
        });

        nextCalled.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(expectedId);
    }

    /// <summary>
    /// Cuando la validación falla, el siguiente handler no debe ser invocado.
    /// </summary>
    [Fact]
    public async Task HandleConValidacionFallidaNoLlamaNextHandlerAsync()
    {
        var command = new CreateAccountCommand("", 1_000m, "DOP");
        bool nextCalled = false;

        _validator
            .When(v => v.Validate(Arg.Any<CreateAccountCommand>()))
            .Do(_ => throw new ValidationException(new Dictionary<string, List<string>>
            {
                { "Name", ["El nombre es requerido."] }
            }));

        Result<Guid> result = await _behavior.Handle(command, () =>
        {
            nextCalled = true;
            return Task.FromResult(Result.Success(Guid.NewGuid()));
        });

        nextCalled.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();
    }

    /// <summary>
    /// Cuando la validación falla, el error debe contener el código, tipo y metadata correctos.
    /// </summary>
    [Fact]
    public async Task HandleConValidacionFallidaRetornaErrorConMetadataAsync()
    {
        var command = new CreateAccountCommand("", 1_000m, "DOP");
        var erroresEsperados = new Dictionary<string, List<string>>
        {
            { "Name", ["El nombre es requerido."] }
        };

        _validator
            .When(v => v.Validate(Arg.Any<CreateAccountCommand>()))
            .Do(_ => throw new ValidationException(erroresEsperados));

        Result<Guid> result = await _behavior.Handle(command, () =>
            Task.FromResult(Result.Success(Guid.NewGuid())));

        result.Error.Code.ShouldBe("Validation.Failed");
        result.Error.Type.ShouldBe(ErrorType.Validation);
        result.Error.Metadata.ShouldNotBeNull();
        result.Error.Metadata.ShouldContainKey("Name");
    }
}
