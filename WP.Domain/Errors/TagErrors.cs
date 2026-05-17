namespace WP.Domain.Errors;

/// <summary>
/// Errores de dominio relacionados con los tags de transacción.
/// </summary>
public static class TagErrors
{
    /// <summary>
    /// Error cuando el nombre del tag es nulo o vacío.
    /// </summary>
    public static Error NombreVacio() =>
        Error.Validation("Tag.NombreVacio", "El nombre del tag no puede ser vacío.");

    /// <summary>
    /// Error cuando el nombre del tag supera el máximo de caracteres permitidos.
    /// </summary>
    /// <param name="maxLength">La longitud máxima permitida.</param>
    public static Error NombreMuyLargo(int maxLength) =>
        Error.Validation("Tag.NombreMuyLargo", $"El nombre del tag no puede superar {maxLength} caracteres.");
}
