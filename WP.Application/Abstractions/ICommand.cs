namespace WP.Application.Abstractions;

/// <summary>
/// Define un contrato para los comandos que devuelven un resultado de la aplicación.
/// </summary>
/// <typeparam name="TResult">El tipo del resultado que devuelve el comando.</typeparam>
[SuppressMessage("Design", "CA1040:Interfaces should not be empty",
    Justification = "Este contrato se utiliza para marcar los comandos sin resultado.")]
[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed",
    Justification = "TResult es un marcador que obliga al handler a retornar el tipo correcto.")]
public interface ICommand<TResult>;
