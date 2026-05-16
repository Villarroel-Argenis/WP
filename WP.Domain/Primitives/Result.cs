namespace WP.Domain.Primitives;

/// <summary>
/// Clase estática para crear instancias de Result sin especificar el tipo genérico.
/// </summary>
public static class Result
{
    /// <summary>
    /// Crea un resultado exitoso.
    /// </summary>
    /// <typeparam name="TValue">Tipo del valor.</typeparam>
    /// <param name="value">El valor del resultado.</param>
    /// <returns>Un resultado exitoso.</returns>
    public static Result<TValue> Success<TValue>(TValue value) =>
        Result<TValue>.Success(value);

    /// <summary>
    /// Crea un resultado fallido.
    /// </summary>
    /// <typeparam name="TValue">Tipo del valor.</typeparam>
    /// <param name="error">El error del resultado.</param>
    /// <returns>Un resultado fallido.</returns>
    public static Result<TValue> Failure<TValue>(Error error) =>
        Result<TValue>.Failure(error);
}

/// <summary>
/// Representa el resultado de una operación que puede fallar.
/// </summary>
/// <typeparam name="TValue">Tipo del valor en caso de éxito.</typeparam>
public sealed class Result<TValue>
{
    private readonly TValue? _value;

    internal Result(TValue value)
    {
        _value = value;
        Error = Error.None;
        IsSuccess = true;
    }

    internal Result(Error error)
    {
        _value = default;
        Error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica si la operación falló.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Obtiene el valor en caso de éxito.
    /// </summary>
    /// <exception cref="InvalidOperationException">Si el resultado es fallido.</exception>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            "No se puede acceder al valor de un resultado fallido.");

    /// <summary>
    /// Obtiene el error en caso de fallo.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Crea un resultado exitoso.
    /// </summary>
    /// <param name="value">El valor del resultado.</param>
    internal static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Crea un resultado fallido.
    /// </summary>
    /// <param name="error">El error del resultado.</param>
    internal static Result<TValue> Failure(Error error) => new(error);

    /// <summary>
    /// Conversión implícita de valor a resultado exitoso.
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => new(value);

    /// <summary>
    /// Conversión implícita de error a resultado fallido.
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => new(error);
}
