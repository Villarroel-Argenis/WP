namespace WP.Domain.Tests;

/// <summary>
/// Pruebas unitarias para la entidad Transaction.
/// </summary>
public sealed class TransactionTests
{
    /// <summary>
    /// Verifica que crear un ingreso con datos válidos retorna una transacción correcta.
    /// </summary>
    [Fact]
    public void CreateIncomeConDatosValidosRetornaTransaccion()
    {
        var amount = Money.Of(1000, Currency.Dop);

        var transaction = Transaction.CreateIncome(Guid.NewGuid(), amount, "Salario");

        transaction.ShouldNotBeNull();
        transaction.Amount.ShouldBe(amount);
        transaction.Type.ShouldBe(TransactionType.Income);
        transaction.Description.ShouldBe("Salario");
        transaction.Tags.ShouldBeEmpty();
    }

    /// <summary>
    /// Verifica que crear un ingreso con tags retorna una transacción con las etiquetas correctas.
    /// </summary>
    [Fact]
    public void CreateIncomeConTagsRetornaTransaccionConTags()
    {
        var amount = Money.Of(1000, Currency.Dop);

        var transaction = Transaction.CreateIncome(Guid.NewGuid(), amount, "Salario", ["Salario", "Quincena"]);

        transaction.Tags.Count.ShouldBe(2);
        transaction.Tags.ShouldContain(t => t == Tag.From("Salario"));
        transaction.Tags.ShouldContain(t => t == Tag.From("Quincena"));
    }

    /// <summary>
    /// Verifica que crear un gasto con datos válidos retorna una transacción correcta.
    /// </summary>
    [Fact]
    public void CreateExpenseConDatosValidosRetornaTransaccion()
    {
        var amount = Money.Of(500, Currency.Dop);

        var transaction = Transaction.CreateExpense(Guid.NewGuid(), amount, "Compra de supermercado");

        transaction.Type.ShouldBe(TransactionType.Expense);
        transaction.ShouldNotBeNull();
    }

    /// <summary>
    /// Verifica que crear una transferencia con datos válidos retorna una transacción correcta.
    /// </summary>
    [Fact]
    public void CreateTransferConDatosValidosRetornaTransaccion()
    {
        var amount = Money.Of(500m, Currency.Dop);
        var transferId = Guid.NewGuid();

        var transaction = Transaction.CreateTransfer(Guid.NewGuid(), amount, transferId);

        transaction.Type.ShouldBe(TransactionType.Transfer);
        transaction.TransferId.ShouldBe(transferId);
    }

    /// <summary>
    /// Verifica que crear una transferencia con TransferId vacío lanza ArgumentException.
    /// </summary>
    [Fact]
    public void CreateTransferConTransferIdVacioLanzaArgumentException()
    {
        var amount = Money.Of(500m, Currency.Dop);

        Should.Throw<ArgumentException>(() =>
            Transaction.CreateTransfer(Guid.NewGuid(), amount, Guid.Empty));
    }

    /// <summary>
    /// Verifica que crear un ingreso con monto nulo lanza ArgumentNullException.
    /// </summary>
    [Fact]
    public void CreateIncomeConMontoNuloLanzaArgumentNullException()
    {
        Should.Throw<ArgumentNullException>(() =>
            Transaction.CreateIncome(Guid.NewGuid(), null!));
    }
}
