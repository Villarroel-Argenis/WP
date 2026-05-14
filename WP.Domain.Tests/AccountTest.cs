namespace WP.Domain.Tests;

/// <summary>
/// Pruebas unitarias para la clase Account.
/// </summary>
public sealed class AccountTest
{
    /// <summary>
    /// Verifica que la creación de una cuenta con datos válidos devuelva una instancia de Account.
    /// </summary>
    [Fact]
    public void CreateWithValidDataReturnsAccount()
    {
        var balance = Money.Of(1000, Currency.Dop);
        var account = Account.Create("Cuenta de Ahorros", balance);

        account.ShouldNotBeNull();
        account.Name.ShouldBe("Cuenta de Ahorros");
        account.Balance.ShouldBe(balance);
        account.Id.ShouldBeOfType<Guid>();
    }

    /// <summary>
    /// Verifica que la creación de una cuenta con un nombre inválido lance una ArgumentException.
    /// </summary>
    /// <param name="name">El nombre inválido para la cuenta.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateWithInvalidNameThrowsArgumentException(string? name)
    {
        var balance = Money.Of(1000, Currency.Dop);

        Should.Throw<ArgumentException>(() => Account.Create(name!, balance))
            .Message.ShouldBe("El nombre de la cuenta no puede ser vacio. (Parameter 'name')");
    }
}
