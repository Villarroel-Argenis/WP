namespace WP.Infrastructure.Dispatchers;

/// <summary>
/// Implementación del despachador de comandos con pipeline de behaviors.
/// </summary>
public sealed class CommandDispatcher(IServiceProvider serviceProvider)
    : ICommandDispatcher
{
    /// <summary>
    /// Despacha un comando a través del pipeline de behaviors.
    /// </summary>
    /// <typeparam name="TCommand">Tipo del comando.</typeparam>
    /// <typeparam name="TResult">Tipo del resultado.</typeparam>
    /// <param name="command">El comando a despachar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El resultado del comando.</returns>
    public Task<TResult> SendAsyn<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
    {
        ICommandHandler<TCommand, TResult> handler = serviceProvider
            .GetRequiredService<ICommandHandler<TCommand, TResult>>();

        var behaviors = serviceProvider
            .GetServices<IPipelineBehavior<TCommand, TResult>>()
            .Reverse()
            .ToList();

        CommandHandlerNext<TResult> pipeline = () =>
            handler.Handle(command, cancellationToken);

        foreach (IPipelineBehavior<TCommand, TResult> behavior in behaviors)
        {
            CommandHandlerNext<TResult> next = pipeline;
            IPipelineBehavior<TCommand, TResult> current = behavior;
            pipeline = () => current.Handle(command, next, cancellationToken);
        }

        return pipeline();
    }
}
