namespace WP.Domain.Transactions;

/// <summary>
/// Errores de dominio relacionados con las transacciones.
/// </summary>
public static class TransactionErrors
{
    /// <summary>
    /// Error cuando el tipo de transacción recibido no es válido.
    /// </summary>
    /// <param name="tipo">El tipo inválido recibido.</param>
    public static Error TipoInvalido(string tipo) =>
        Error.Validation("Transaction.TipoInvalido", $"El tipo de transacción '{tipo}' no es válido.");

    /// <summary>
    /// Error cuando se intenta una transferencia sin especificar la cuenta destino.
    /// </summary>
    public static Error CuentaObjetivoRequerida() =>
        Error.Validation("Transaction.CuentaObjetivoRequerida", "La cuenta destino es requerida para transferencias.");

    /// <summary>
    /// Error cuando el identificador de transferencia está vacío.
    /// </summary>
    public static Error TransferIdVacio() =>
        Error.Validation("Transaction.TransferIdVacio", "El identificador de transferencia no puede ser vacío.");

    /// <summary>
    /// Error cuando no se encuentra una transacción con el identificador especificado.
    /// </summary>
    /// <param name="id">El identificador de la transacción no encontrada.</param>
    public static Error NotFound(Guid id) =>
        Error.NotFound("Transaction.NotFound", $"No se encontró la transacción con id '{id}'.");
}
