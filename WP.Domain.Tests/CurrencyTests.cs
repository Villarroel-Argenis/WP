namespace WP.Domain.Tests;


/// <summary>
/// Pruebas unitarias para la clase Currency, que verifican la correcta creación de instancias de Currency a partir de códigos de moneda válidos y la adecuada gestión de casos con códigos inválidos, asegurando que el método From funcione correctamente tanto para entradas válidas como para entradas que deberían generar excepciones. Estas pruebas son fundamentales para garantizar la integridad y confiabilidad de la funcionalidad relacionada con las monedas en el dominio de la aplicación.
/// </summary>
public sealed class CurrencyTests
{
    /// <summary>
    /// Verifica que el método From con un código de moneda válido retorna una instancia de Currency con el código correcto, asegurando que la creación de objetos Currency a partir de códigos de moneda funcione correctamente para los códigos "DOP", "EUR" y "USD".
    /// </summary>
    /// <param name="code"></param>
    [Theory]
    [InlineData("DOP")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void FromConCodigoRetirnaValido(string? code)
    {
        Result<Currency> currency = Currency.From(code!);

        currency.Value.Code.ShouldBe(code);
    }

    /// <summary>
    /// Verifica que el método From con códigos de moneda vacíos, nulos o no válidos lanza una ArgumentException, asegurando que la validación de códigos de moneda funcione correctamente y que se manejen adecuadamente los casos en los que el código es inválido, como cadenas vacías, códigos con menos o más de 3 caracteres, o códigos no reconocidos.
    /// </summary>
    /// <param name="code"></param>
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("US")]
    [InlineData("DOPA")]
    public void FromConCodigosVaciosOIvalidosRetornaArgumentException(string? code) => Currency.From(code!).IsFailure.ShouldBeTrue();
}
