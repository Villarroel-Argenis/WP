namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el validador de comandos CreateAccountCommandValidator.
/// </summary>
public class CreateAccountCommandValidatorTests
{
    private readonly CreateAccountCommandValidator _validator = new();

    /// <summary>
    /// Verifica que la validación con datos válidos no lanza ninguna excepción.
    /// </summary>
    [Fact]
    public void ValidateConDatosValidosNoLanzaException()
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, "DOP");

        Should.NotThrow(() => _validator.Validate(command));
    }


    /// <summary>
    /// Verifica que la validación con nombres vacíos o nulos lanza ValidationException.
    /// </summary>
    /// <param name="name"></param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateConNombresVaciosONuloLanzaLaValidationException(string? name)
    {
        var command = new CreateAccountCommand(name!, 1_000m, "DOP");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("Name").ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que la validación con nombres mayores a 100 caracteres lanza ValidationException.
    /// </summary>
    [Fact]
    public void ValidateConNombreMayorA100CaracteresLanzaValidationException()
    {
        string name = new('A', 101);
        var command = new CreateAccountCommand(name, 1_000m, "DOP");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey(nameof(CreateAccountCommand.Name)).ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que la validación con montos negativos lanza ValidationException.
    /// </summary>
    [Fact]
    public void ValidateConMontoNegativoLanzaValidationException()
    {
        var command = new CreateAccountCommand("Ahorros", -1_000m, "DOP");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey(nameof(CreateAccountCommand.InitialAmount)).ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que la validación con códigos de moneda vacíos, nulos o no validas lanza ValidationException.
    /// </summary>
    /// <param name="code"></param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("US")]
    [InlineData("DOPA")]
    public void ValidateConMonedaVaciaOInvalidasLanzaValidationException(string? code)
    {
        var command = new CreateAccountCommand("Ahorros", 1_000m, code!);

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));
        ex.Errors.ContainsKey(nameof(CreateAccountCommand.CurrencyCode)).ShouldBeTrue();
    }
}
