namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el behavior de validación del pipeline.
/// </summary>
public sealed class ValidationBehaviorTests
{
    private readonly IValidator<CreateAccountCommand> _validator;
    private readonly ValidationBehavior<CreateAccountCommand, Guid> _behavior;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    public ValidationBehaviorTests()
    {
        _validator = Substitute.For<IValidator<CreateAccountCommand>>();
        _behavior = new ValidationBehavior<CreateAccountCommand, Guid>(_validator);
    }

    /// <summary>
    /// Verifica que con datos válidos llama al siguiente paso del pipeline.
    /// </summary>
    [Fact]
    public async Task HandleConDatosValidosLlamaNextHandlerAsync()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");
        bool nextCalled = false;

        await _behavior.Handle(
            command,
            () =>
            {
                nextCalled = true;
                return Task.FromResult(Result.Success(Guid.NewGuid()));
            },
            CancellationToken.None);

        nextCalled.ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que cuando la validación falla no llama al siguiente paso.
    /// </summary>
    [Fact]
    public async Task HandleConValidacionFallidaNoLlamaNextHandlerAsync()
    {
        var command = new CreateAccountCommand("", 1_000m, "DOP");
        bool nextCalled = false;

        _validator
            .When(v => v.Validate(Arg.Any<CreateAccountCommand>()))
            .Do(_ => throw new ValidationException(
                new Dictionary<string, List<string>>
                {
                    { "Name", ["El nombre es requerido."] }
                }));

        await Should.ThrowAsync<ValidationException>(() =>
            _behavior.Handle(
                command,

                () =>
                {
                    nextCalled = true;
                    return Task.FromResult(Result.Success(Guid.NewGuid()));
                },
                CancellationToken.None));

        nextCalled.ShouldBeFalse();
    }

    /// <summary>
    /// Verifica que sin validator registrado llama al siguiente paso directamente.
    /// </summary>
    [Fact]
    public async Task HandleSinValidatorLlamaNextHandlerDirectamenteAsync()
    {
        var behaviorSinValidator = new ValidationBehavior<CreateAccountCommand, Guid>(new CreateAccountCommandValidator());
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");
        bool nextCalled = false;

        await behaviorSinValidator.Handle(
            command,
            () =>
            {
                nextCalled = true;
                return Task.FromResult(Result.Success(Guid.NewGuid()));
            },
            CancellationToken.None);

        nextCalled.ShouldBeTrue();
    }
}
