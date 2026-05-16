namespace WP.Application.Abstractions;

/// <summary>
/// Delegado que representa el siguiente paso en el pipeline de un comando.
/// </summary>
/// <typeparam name="TResult">Tipo del resultado del comando.</typeparam>
public delegate Task<Result<TResult>> CommandHandlerNext<TResult>();
