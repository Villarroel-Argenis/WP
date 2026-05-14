namespace WP.Application.Abstractions;

/// <summary>
/// Define un contrato para las consultas de la aplicación que devuelven un resultado.
/// </summary>
/// <typeparam name="TResult">El tipo del resultado que devuelve la consulta.</typeparam>
[SuppressMessage("Design", "CA1040:Interfaces should not be empty", Justification = "Este contrato se utiliza para marcar las consultas con resultado.")]
[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "TResult es un marcador que obliga al handler a retornar el tipo correcto.")]
public interface IQuery<TResult>;
