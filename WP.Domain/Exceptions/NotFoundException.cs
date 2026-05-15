namespace WP.Domain.Exceptions;


/// <summary>
/// Excepción que se lanza cuando no se encuentra una entidad en el sistema.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/> con un mensaje personalizado que indica que la entidad con el identificador especificado no fue encontrada.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="id"></param>
    public NotFoundException(string entity, Guid id)
    : base($"{entity} con id '{id}' no fue encontrada")
    {
    }
}
