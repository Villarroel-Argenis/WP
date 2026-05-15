namespace WP.Application.Abstractions;

/// <summary>
/// Interfaz que define un validador genérico para validar objetos de tipo T.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IValidator<in T>
{
    /// <summary>
    /// Valida el objeto de tipo T y lanza una excepción de validación si el objeto no cumple con los criterios de validación.
    /// </summary>
    /// <param name="value"></param>
    void Validate(T value);
}
