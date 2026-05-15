namespace WP.Domain.Tests;


/// <summary>
/// Contiene pruebas unitarias para la clase Money, verificando el comportamiento de las operaciones de suma y resta, así como la validación de monedas y fondos suficientes.
/// </summary>
public sealed class MoneyTests
{
    /// <summary>
    /// Verifica que la operación de suma con la misma moneda retorna la suma correcta del monto y mantiene la misma moneda.
    /// </summary>
    [Fact]
    public void AddConMismaMonedaRetornaSuma()
    {
        var a = Money.Of(500m, Currency.Dop);
        var b = Money.Of(300m, Currency.Dop);

        Money result = a.Add(b);

        result.Amount.ShouldBe(800m);
        result.Currency.ShouldBe(Currency.Dop);
    }

    /// <summary>
    /// Verifica que la operación de suma con fondos insuficientes lanza una InvalidOperationException, indicando que no se pueden sumar montos cuando el monto del primer objeto es menor que el monto del segundo objeto.
    /// </summary>
    [Fact]
    public void AddConDistintaMonedaLanzaInvalidOperationException()
    {
        var a = Money.Of(500m, Currency.Dop);
        var b = Money.Of(300m, Currency.Usd);

        Should.Throw<InvalidOperationException>(() => a.Add(b));
    }

    /// <summary>
    /// Verifica que la operación de resta con la misma moneda retorna la diferencia correcta del monto y mantiene la misma moneda, siempre y cuando el monto del primer objeto sea mayor o igual al monto del segundo objeto.
    /// </summary>
    [Fact]
    public void SubtractConMismaMonedaRetornaDiferencia()
    {
        var a = Money.Of(500m, Currency.Dop);
        var b = Money.Of(300m, Currency.Dop);

        Money result = a.Subtract(b);

        result.Amount.ShouldBe(200m);
        result.Currency.ShouldBe(Currency.Dop);
    }

    /// <summary>
    /// Verifica que la operación de resta con fondos insuficientes lanza una InvalidOperationException, indicando que no se pueden restar montos cuando el monto del primer objeto es menor que el monto del segundo objeto.
    /// </summary>
    [Fact]
    public void SubtractConFondosInsuficientesLanzaInvalidOperationException()
    {
        var a = Money.Of(100m, Currency.Dop);
        var b = Money.Of(300m, Currency.Dop);

        Should.Throw<InvalidOperationException>(() => a.Subtract(b));
    }

    /// <summary>
    /// Verifica que la operación de resta con monedas diferentes lanza una InvalidOperationException, indicando que no se pueden restar montos con diferentes monedas.
    /// </summary>
    [Fact]
    public void SubtractConDistintaMonedaLanzaInvalidOperationException()
    {
        var a = Money.Of(500m, Currency.Dop);
        var b = Money.Of(300m, Currency.Usd);

        Should.Throw<InvalidOperationException>(() => a.Subtract(b));
    }
}
