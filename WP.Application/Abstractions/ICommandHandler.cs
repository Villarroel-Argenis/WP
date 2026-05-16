namespace WP.Application.Abstractions;

/// <summary>
/// Define un contrato para los manejadores de comandos que devuelven un resultado.
/// </summary>
/// <typeparam name="TCommand">El tipo del comando a manejar.</typeparam>
/// <typeparam name="TResult">El tipo del resultado que devuelve el manejador.</typeparam>
[SuppressMessage(
    "Major Code Smell", "S2326:Unused type parameters should be removed",
    Justification = "TResult es un marcador que obliga al handler a retornar el tipo correcto.")]
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Maneja de forma asíncrona el comando especificado y devuelve un resultado.
    /// </summary>
    /// <param name="command">El comando a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el resultado.</returns>
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
