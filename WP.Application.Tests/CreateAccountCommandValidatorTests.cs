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
}
