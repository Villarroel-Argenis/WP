namespace WP.Application.Abstractions;

/// <summary>
/// Excepción personalizada que se lanza cuando se encuentran errores de validación en la aplicación. Contiene un diccionario de errores donde la clave es el nombre del campo y el valor es un array de mensajes de error asociados a ese campo.
/// </summary>
public sealed class ValidationException : Exception
{
    /// <summary>
    /// Diccionario de errores de validación, donde la clave es el nombre del campo y el valor es un array de mensajes de error asociados a ese campo.
    /// </summary>
    public IReadOnlyDictionary<string, List<string>> Errors { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase ValidationException con un diccionario de errores de validación. El mensaje de la excepción se establece en "Se encontraron errores de validacion.".
    /// </summary>
    /// <param name="errors"></param>
    public ValidationException(IReadOnlyDictionary<string, List<string>> errors)
        : base("Se encontraron errores de validacion.")
    {
        Errors = errors;
    }
}
