namespace WP.Domain.Tests;

/// <summary>
/// Pruebas unitarias para el value object Tag.
/// </summary>
public sealed class TagTests
{
    /// <summary>
    /// Verifica que crear un tag con un nombre válido retorna el tag correcto.
    /// </summary>
    [Fact]
    public void FromConNombreValidoRetornaTag()
    {
        var tag = Tag.From("Salario");

        tag.Name.ShouldBe("salario");
    }

    /// <summary>
    /// Verifica que crear un tag con nombre válido normaliza a minúsculas.
    /// </summary>
    [Fact]
    public void FromNormalizaElNombreAMinusculas()
    {
        var tag = Tag.From("  TRABAJO  ");

        tag.Name.ShouldBe("trabajo");
    }

    /// <summary>
    /// Verifica que crear un tag con nombre vacío lanza ArgumentException.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void FromConNombreVacioLanzaArgumentException(string? name)
    {
        Should.Throw<ArgumentException>(() => Tag.From(name!));
    }

    /// <summary>
    /// Verifica que crear un tag con nombre mayor a 50 caracteres lanza ArgumentException.
    /// </summary>
    [Fact]
    public void FromConNombreMayorA50CaracteresLanzaArgumentException()
    {
        string name = new string('a', 51);

        Should.Throw<ArgumentException>(() => Tag.From(name));
    }

    /// <summary>
    /// Verifica que dos tags con el mismo nombre son iguales.
    /// </summary>
    [Fact]
    public void DosTagsConMismoNombreSonIguales()
    {
        var tag1 = Tag.From("salario");
        var tag2 = Tag.From("salario");

        tag1.ShouldBe(tag2);
    }

    /// <summary>
    /// Verifica que dos tags con diferente nombre no son iguales.
    /// </summary>
    [Fact]
    public void DosTagsConDiferenteNombreNoSonIguales()
    {
        var tag1 = Tag.From("salario");
        var tag2 = Tag.From("trabajo");

        tag1.ShouldNotBe(tag2);
    }
}
