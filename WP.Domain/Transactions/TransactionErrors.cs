namespace WP.Domain.Transactions;

/// <summary>
/// Define los errores de dominio relacionados con Transaction.
/// </summary>
public static class TransactionErrors
{
    /// <summary>
    /// Error cuando el tipo de transacción es inválido.
    /// </summary>
    public static Error InvalidType(string type) =>
        Error.Validation(
            "Transaction.InvalidType",
            $"Tipo de transacción inválido: {type}.");

    /// <summary>
    /// Error cuando se intenta una transferencia sin cuenta destino.
    /// </summary>
    public static readonly Error MissingTargetAccount =
        Error.Validation(
            "Transaction.MissingTargetAccount",
            "La cuenta destino es requerida para transferencias.");
}
