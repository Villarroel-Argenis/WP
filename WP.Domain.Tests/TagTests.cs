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
        Result<Tag> tag = Tag.From("Salario");

        tag.Value.Name.ShouldBe("salario");
    }

    /// <summary>
    /// Verifica que crear un tag con nombre válido normaliza a minúsculas.
    /// </summary>
    [Fact]
    public void FromNormalizaElNombreAMinusculas()
    {
        Result<Tag> tag = Tag.From("  TRABAJO  ");

        tag.Value.Name.ShouldBe("trabajo");
    }

    /// <summary>
    /// Verifica que crear un tag con nombre vacío lanza ArgumentException.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void FromConNombreVacioLanzaArgumentException(string? name) => Tag.From(name!).IsFailure.ShouldBeTrue();

    /// <summary>
    /// Verifica que crear un tag con nombre mayor a 50 caracteres lanza ArgumentException.
    /// </summary>
    [Fact]
    public void FromConNombreMayorA50CaracteresLanzaArgumentException()
    {
        string name = new('a', 51);

        Result<Tag> result = Tag.From(name);

        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Tag.NombreMuyLargo");
    }

    /// <summary>
    /// Verifica que dos tags con el mismo nombre son iguales.
    /// </summary>
    [Fact]
    public void DosTagsConMismoNombreSonIguales()
    {
        Tag tag1 = Tag.From("salario").Value;
        Tag tag2 = Tag.From("salario").Value;

        tag1.ShouldBe(tag2);
    }

    /// <summary>
    /// Verifica que dos tags con diferente nombre no son iguales.
    /// </summary>
    [Fact]
    public void DosTagsConDiferenteNombreNoSonIguales()
    {
        Result<Tag> tag1 = Tag.From("salario");
        Result<Tag> tag2 = Tag.From("trabajo");

        tag1.ShouldNotBe(tag2);
    }
}
