namespace WP.Application.Tests;

/// <summary>
/// Pruebas unitarias para el validador de registro de transacciones.
/// </summary>
public sealed class RegisterTransactionCommandValidatorTests
{
    private readonly RegisterTransactionCommandValidator _validator = new();

    /// <summary>
    /// Verifica que un ingreso con datos válidos no lanza excepción.
    /// </summary>
    [Fact]
    public void ValidateConIngresoValidoNoLanzaExcepcion()
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 500m, "DOP", "Income", "Salario");

        Should.NotThrow(() => _validator.Validate(command));
    }

    /// <summary>
    /// Verifica que un gasto con datos válidos no lanza excepción.
    /// </summary>
    [Fact]
    public void ValidateConGastoValidoNoLanzaExcepcion()
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 300m, "DOP", "Expense", "Supermercado");

        Should.NotThrow(() => _validator.Validate(command));
    }

    /// <summary>
    /// Verifica que una transferencia con datos válidos no lanza excepción.
    /// </summary>
    [Fact]
    public void ValidateConTransferenciaValidaNoLanzaExcepcion()
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 200m, "DOP", "Transfer",
            TargetAccountId: Guid.NewGuid());

        Should.NotThrow(() => _validator.Validate(command));
    }

    /// <summary>
    /// Verifica que un AccountId vacío lanza ValidationException.
    /// </summary>
    [Fact]
    public void ValidateConAccountIdVacioLanzaValidationException()
    {
        var command = new RegisterTransactionCommand(
            Guid.Empty, 500m, "DOP", "Income");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("AccountId").ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que un monto menor o igual a cero lanza ValidationException.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void ValidateConMontoInvalidoLanzaValidationException(decimal amount)
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), amount, "DOP", "Income");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("Amount").ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que un código de moneda inválido lanza ValidationException.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("US")]
    [InlineData("DOPA")]
    [InlineData(null)]
    public void ValidateConMonedaInvalidaLanzaValidationException(string? code)
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 500m, code!, "Income");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("CurrencyCode").ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que un tipo de transacción inválido lanza ValidationException.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData(null)]
    public void ValidateConTipoInvalidoLanzaValidationException(string? type)
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 500m, "DOP", type!);

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("Type").ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que una transferencia sin cuenta destino lanza ValidationException.
    /// </summary>
    [Fact]
    public void ValidateConTransferenciasinCuentaDestinoLanzaValidationException()
    {
        var command = new RegisterTransactionCommand(
            Guid.NewGuid(), 500m, "DOP", "Transfer");

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("TargetAccountId").ShouldBeTrue();
    }

    /// <summary>
    /// Verifica que una transferencia con cuenta destino igual a cuenta origen lanza ValidationException.
    /// </summary>
    [Fact]
    public void ValidateConCuentaDestinoIgualACuentaOrigenLanzaValidationException()
    {
        var accountId = Guid.NewGuid();
        var command = new RegisterTransactionCommand(
            accountId, 500m, "DOP", "Transfer",
            TargetAccountId: accountId);

        ValidationException ex = Should.Throw<ValidationException>(() => _validator.Validate(command));

        ex.Errors.ContainsKey("TargetAccountId").ShouldBeTrue();
    }
}
