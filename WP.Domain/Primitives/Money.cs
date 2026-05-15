namespace WP.Domain.Primitives;

/// <summary>
/// Representa una cantidad de dinero con un monto y una moneda.
/// </summary>
/// <param name="Amount">El monto decimal del dinero.</param>
/// <param name="Currency">La moneda del dinero.</param>
public sealed record Money(decimal Amount, Currency Currency)
{
    /// <summary>
    /// Crea una instancia de Money con el monto y moneda especificados.
    /// </summary>
    /// <param name="amount">El monto decimal.</param>
    /// <param name="currency">La moneda.</param>
    /// <returns>Una nueva instancia de Money.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si la moneda es nula.</exception>
    /// <exception cref="ArgumentException">Se lanza si el monto es negativo.</exception>
    public static Money Of(decimal amount, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        return amount < 0 ? throw new ArgumentException("El monto no puede ser negativo.", nameof(amount)) : new Money(amount, currency);
    }

    /// <summary>
    /// Suma este monto con otro monto de la misma moneda.
    /// </summary>
    /// <param name="money">El monto a sumar.</param>
    /// <returns>Un nuevo Money con la suma de los montos.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si las monedas son diferentes.</exception>
    public Money Add(Money money) => Currency != money.Currency ? throw new InvalidOperationException("No se pueden sumar montos con diferentes monedas.") : new Money(Amount + money.Amount, Currency);

    /// <summary>
    /// Resta este monto con otro monto de la misma moneda.
    /// </summary>
    /// <param name="money">El monto a restar.</param>
    /// <returns>Un nuevo Money con la resta de los montos.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si las monedas son diferentes.</exception>
    public Money Subtract(Money money) => Currency != money.Currency ? throw new InvalidOperationException("No se pueden restar montos con diferentes monedas.") : new Money(Amount - money.Amount, Currency);
}
