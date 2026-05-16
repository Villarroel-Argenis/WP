namespace WP.Application.Behaviors;

/// <summary>
/// Behavior que loguea la ejecución de cada comando.
/// </summary>
/// <typeparam name="TCommand">Tipo del comando.</typeparam>
/// <typeparam name="TResult">Tipo del resultado.</typeparam>
public partial class LoggingBehavior<TCommand, TResult>(ILogger<LoggingBehavior<TCommand, TResult>> logger)
    : IPipelineBehavior<TCommand, TResult>
{
    /// <summary>
    /// Loguea el comando y el tiempo de ejecución.
    /// </summary>
    /// <param name="command">El comando a procesar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <param name="nextHandler">El siguiente paso en el pipeline.</param>
    /// <returns>El resultado del comando.</returns>
    public async Task<Result<TResult>> Handle(TCommand command, CommandHandlerNext<TResult> nextHandler, CancellationToken cancellationToken = default)
    {
        string commandName = typeof(TCommand).Name;

        LogEjecutandoComandoCommandname(commandName);

        var stopwatch = Stopwatch.StartNew();

        Result<TResult> result = await nextHandler();

        stopwatch.Stop();

        if (result.IsFailure)
        {
            LogComandoCommandnameConEerorCode(commandName, result.Error.Code);
        }
        else
        {
            LogComandoCommandnameCompletadoEnElapsemsMs(commandName, stopwatch.ElapsedMilliseconds);
        }

        return result;
    }

    [LoggerMessage(LogLevel.Information, "Ejecutando comando {CommandName}")]
    partial void LogEjecutandoComandoCommandname(string commandName);

    [LoggerMessage(LogLevel.Information, "Comando {CommandName} completado en {ElapseMs}ms")]
    partial void LogComandoCommandnameCompletadoEnElapsemsMs(string commandName, long elapseMs);

    [LoggerMessage(LogLevel.Error, "Comando {CommandName} con error: {Code}")]
    partial void LogComandoCommandnameConEerorCode(string commandName, string code);
}
