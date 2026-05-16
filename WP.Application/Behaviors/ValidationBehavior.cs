namespace WP.Application.Behaviors;

/// <summary>
/// Behavior que ejecuta la validación antes de llegar al handler.
/// </summary>
/// <typeparam name="TCommand">Tipo del comando.</typeparam>
/// <typeparam name="TResult">Tipo del resultado.</typeparam>
public class ValidationBehavior<TCommand, TResult>(IValidator<TCommand> validator)
    : IPipelineBehavior<TCommand, TResult>
{
    /// <summary>
    /// Valida el comando antes de continuar el pipeline.
    /// </summary>
    /// <param name="command">El comando a validar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <param name="nextHandler">El siguiente paso en el pipeline.</param>
    /// <returns>El resultado del comando.</returns>
    public Task<TResult> Handle(TCommand command, CommandHandlerNext<TResult> nextHandler, CancellationToken cancellationToken = default)
    {
        validator.Validate(command);
        return nextHandler();
    }
}
