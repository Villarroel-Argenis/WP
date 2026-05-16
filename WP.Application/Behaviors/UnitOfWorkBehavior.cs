namespace WP.Application.Behaviors;

/// <summary>
/// Behavior que persiste los cambios en la base de datos al finalizar el handler,
/// únicamente si el resultado es exitoso.
/// </summary>
/// <typeparam name="TCommand">Tipo del comando.</typeparam>
/// <typeparam name="TResult">Tipo del resultado.</typeparam>
public class UnitOfWorkBehavior<TCommand, TResult>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TCommand, TResult>
{
    /// <summary>
    /// Ejecuta el handler y persiste los cambios si el resultado es exitoso.
    /// </summary>
    /// <param name="command">El comando a ejecutar.</param>
    /// <param name="nextHandler">El siguiente paso en el pipeline.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>El resultado del comando.</returns>
    public async Task<Result<TResult>> Handle(TCommand command, CommandHandlerNext<TResult> nextHandler, CancellationToken cancellationToken = default)
    {
        Result<TResult> result = await  nextHandler();

        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}
