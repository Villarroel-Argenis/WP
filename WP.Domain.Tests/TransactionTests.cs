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

        Transaction transaction = Transaction.CreateIncome(Guid.NewGuid(), amount, "Salario").Value;

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

        Transaction transaction = Transaction.CreateIncome(Guid.NewGuid(), amount, "Salario", [Tag.From("Salario").Value, Tag.From("Quincena").Value]).Value;

        transaction.Tags.Count.ShouldBe(2);
        transaction.Tags.ShouldContain(t => t == Tag.From("Salario").Value);
        transaction.Tags.ShouldContain(t => t == Tag.From("Quincena").Value);
    }

    /// <summary>
    /// Verifica que crear un gasto con datos válidos retorna una transacción correcta.
    /// </summary>
    [Fact]
    public void CreateExpenseConDatosValidosRetornaTransaccion()
    {
        var amount = Money.Of(500, Currency.Dop);

        Transaction transaction = Transaction.CreateExpense(Guid.NewGuid(), amount, "Compra de supermercado").Value;

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

        Transaction transaction = Transaction.CreateTransfer(Guid.NewGuid(), amount, transferId).Value;

        transaction.Type.ShouldBe(TransactionType.Transfer);
        transaction.TransferId.ShouldBe(transferId);
    }

    /// <summary>
    /// Verifica que crear una transferencia con TransferId vacío lanza ArgumentException.
    /// </summary>
    [Fact]
    public void CreateTransferConTransferIdVacioRetornaError()
    {
        var amount = Money.Of(500m, Currency.Dop);

        Result<Transaction> result = Transaction.CreateTransfer(Guid.NewGuid(), amount, Guid.Empty);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Transaction.TransferIdVacio");
    }
}
