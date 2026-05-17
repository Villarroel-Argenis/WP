namespace WP.Domain.Primitives;

/// <summary>
/// Representa una etiqueta asociada a una transacción.
/// </summary>
public sealed record Tag
{
    /// <summary>
    /// Obtiene el nombre del tag.
    /// </summary>
    public string Name { get; }

    [ExcludeFromCodeCoverage]
    private Tag(string name) => Name = name;

    /// <summary>
    /// Crea un tag a partir de un nombre.
    /// </summary>
    /// <param name="name">El nombre del tag.</param>
    /// <returns>Una nueva instancia de Tag.</returns>
    public static Result<Tag> From(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Tag>(TagErrors.NombreVacio());
        }

        if (name.Length > 50)
        {
            return Result.Failure<Tag>(TagErrors.NombreMuyLargo(50));
        }

        return new Tag(name.Trim().ToLowerInvariant());
    }
}
