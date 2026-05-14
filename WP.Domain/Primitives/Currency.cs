namespace WP.Domain.Primitives;

/// <summary>
/// Representa una moneda con un código ISO de tres letras.
/// </summary>
public sealed record Currency
{
    /// <summary>
    /// Obtiene el código de moneda de tres letras.
    /// </summary>
    public string Code { get; }

    private Currency(string code) => Code = code;

    /// <summary>
    /// Representa la moneda del dólar estadounidense.
    /// </summary>
    public static readonly Currency Usd = new("USD");

    /// <summary>
    /// Representa la moneda del euro.
    /// </summary>
    public static readonly Currency Eur = new("EUR");

    /// <summary>
    /// Representa la moneda del peso dominicano.
    /// </summary>
    public static readonly Currency Dop = new("DOP");

    /// <summary>
    /// Crea una nueva instancia de Currency a partir de un código de cadena.
    /// </summary>
    /// <param name="code">El código de moneda de tres letras.</param>
    /// <returns>Una nueva instancia de Currency con el código especificado.</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el código es nulo, vacío o no tiene exactamente 3 caracteres.</exception>
    public static Currency From(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("El codigo de la moneda no puede ser vacio.", nameof(code));
        }

        string normalizedCode = code.Trim().ToUpperInvariant();

        return normalizedCode.Length != 3 ? throw new ArgumentException("El codigo de la moneda debe tener exactamente 3 caracteres.", nameof(code)) : new Currency(normalizedCode);
    }
}
