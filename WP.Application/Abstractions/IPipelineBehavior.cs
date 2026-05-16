namespace WP.Application.Abstractions;

/// <summary>
/// Contrato para los behaviors del pipeline de comandos.
/// </summary>
/// <typeparam name="TCommand">Tipo del comando.</typeparam>
/// <typeparam name="TResult">Tipo del resultado.</typeparam>
public interface IPipelineBehavior<in TCommand, TResult>
{
    /// <summary>
    /// Maneja el behavior en el pipeline.
    /// </summary>
    /// <param name="command">El comando a procesar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <param name="nextHandler">El siguiente paso en el pipeline.</param>
    /// <returns>El resultado del comando.</returns>
    Task<Result<TResult>> Handle(
        TCommand command,
        CommandHandlerNext<TResult> nextHandler,
        CancellationToken cancellationToken =  default);
}
