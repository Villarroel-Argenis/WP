namespace WP.Domain.Tests;

/// <summary>
/// Pruebas unitarias para los métodos de transacción de Account.
/// </summary>
public sealed class AccountTransactionTests
{
    /// <summary>
    /// Verifica que aplicar un ingreso aumenta el balance correctamente.
    /// </summary>
    [Fact]
    public void ApplyIncomeAumentaElBalance()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));

        account.ApplyIncome(Money.Of(500m, Currency.Dop));

        account.Balance.Amount.ShouldBe(1_500m);
    }

    /// <summary>
    /// Verifica que aplicar un gasto reduce el balance correctamente.
    /// </summary>
    [Fact]
    public void ApplyExpenseReduceElBalance()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));

        account.ApplyExpense(Money.Of(300m, Currency.Dop));

        account.Balance.Amount.ShouldBe(700m);
    }

    /// <summary>
    /// Verifica que aplicar un gasto con fondos insuficientes lanza InvaliDoperationException.
    /// </summary>
    [Fact]
    public void ApplyExpenseConFondosInsuficientesLanzaInvaliDoperationException()
    {
        var account = Account.Create("Ahorros", Money.Of(100m, Currency.Dop));

        Should.Throw<InvalidOperationException>(() =>
            account.ApplyExpense(Money.Of(500m, Currency.Dop)));
    }

    /// <summary>
    /// Verifica que aplicar un ingreso con monto nulo lanza ArgumentNullException.
    /// </summary>
    [Fact]
    public void ApplyIncomeConMontoNuloLanzaArgumentNullException()
    {
        var account = Account.Create("Ahorros", Money.Of(1_000m, Currency.Dop));

        Should.Throw<ArgumentNullException>(() =>
            account.ApplyIncome(null!));
    }
}
