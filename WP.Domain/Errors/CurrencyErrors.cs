namespace WP.Domain.Errors;

/// <summary>
/// Errores de dominio relacionados con la moneda.
/// </summary>
public static class CurrencyErrors
{
    /// <summary>
    /// Error cuando el código de moneda es nulo o vacío.
    /// </summary>
    public static Error CodigoVacio() =>
        Error.Validation("Currency.CodigoVacio", "El código de la moneda no puede ser vacío.");

    /// <summary>
    /// Error cuando el código de moneda no tiene exactamente 3 caracteres.
    /// </summary>
    /// <param name="code">El código inválido recibido.</param>
    public static Error CodigoInvalido(string code) =>
        Error.Validation("Currency.CodigoInvalido", $"El código '{code}' debe tener exactamente 3 caracteres.");
}
