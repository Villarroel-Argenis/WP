namespace WP.Application.Abstractions;

/// <summary>
/// Clase de extensión que proporciona métodos para agregar errores de validación a un diccionario de errores. Esta clase facilita la construcción de mensajes de error de validación de manera estructurada, permitiendo asociar múltiples mensajes de error a un mismo field. Es especialmente útil en el contexto de validadores que implementan la interfaz IValidator<T>, donde se pueden acumular errores de validación y luego lanzar una excepción con todos los errores encontrados.</T>
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Agrega un mensaje de error a un campo específico en el diccionario de errores. Si el campo no existe en el diccionario, se crea una nueva entrada para ese campo con una lista vacía de mensajes de error, y luego se agrega el mensaje proporcionado a esa lista. Si el campo ya existe, simplemente se agrega el nuevo mensaje a la lista existente de mensajes de error para ese campo.
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="field"></param>
    /// <param name="message"></param>
    public static void AddError(this Dictionary<string, List<string>> errors, string field, string message)
    {
        if (!errors.TryGetValue(field, out List<string>? list))
        {
            list = [];
            errors[field] = list;
        }

        list.Add(message);
    }
}
