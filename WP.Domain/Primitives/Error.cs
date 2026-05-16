namespace WP.Domain.Primitives;

/// <summary>
/// Representa un error de dominio con código y descripción.
/// </summary>
public sealed record Error
{
    /// <summary>
    /// Obtiene el código único del error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Obtiene la descripción del error.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Obtiene el tipo del error.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Data adicional a mostrar
    /// </summary>
    public IReadOnlyDictionary<string, List<string>>? Metadata { get; }
    private Error(string code, string description, ErrorType type, IReadOnlyDictionary<string, List<string>>? metadata = null)
    {
        Code = code;
        Description = description;
        Metadata = metadata;
        Type = type;
    }

    /// <summary>
    /// Crea un error de tipo NotFound.
    /// </summary>
    /// <param name="code">Código del error.</param>
    /// <param name="description">Descripción del error.</param>
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    /// <summary>
    /// Crea un error de tipo Validation.
    /// </summary>
    /// <param name="code">Código del error.</param>
    /// <param name="description">Descripción del error.</param>
    /// <param name="metadata"></param>
    public static Error Validation(string code, string description,
        IReadOnlyDictionary<string, List<string>>? metadata = null) =>
        new(code, description, ErrorType.Validation, metadata);

    /// <summary>
    /// Crea un error de tipo Conflict.
    /// </summary>
    /// <param name="code">Código del error.</param>
    /// <param name="description">Descripción del error.</param>
    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    /// <summary>
    /// Crea un error de tipo Failure.
    /// </summary>
    /// <param name="code">Código del error.</param>
    /// <param name="description">Descripción del error.</param>
    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    /// <summary>
    /// Error vacío, representa ausencia de error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
}
