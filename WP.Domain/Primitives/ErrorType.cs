namespace WP.Domain.Primitives;

/// <summary>
/// Define los tipos de error posibles en el sistema.
/// </summary>
public enum ErrorType
{
    /// <summary>Error de validación de datos.</summary>
    Validation,
    /// <summary>Recurso no encontrado.</summary>
    NotFound,
    /// <summary>Conflicto con el estado actual del recurso.</summary>
    Conflict,
    /// <summary>Error de negocio general.</summary>
    Failure
}
