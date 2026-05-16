namespace WP.Application.Abstractions;

/// <summary>
/// Contrato para el despachador de comandos con pipeline.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Despacha un comando a través del pipeline.
    /// </summary>
    /// <typeparam name="TCommand">Tipo del comando.</typeparam>
    /// <typeparam name="TResult">Tipo del resultado.</typeparam>
    /// <param name="command">El comando a despachar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El resultado del comando.</returns>
    Task<Result<TResult>> SendAsyn<TCommand, TResult>(
        TCommand command,
        CancellationToken  cancellationToken  = default)
        where TCommand : ICommand<TResult>;
}
