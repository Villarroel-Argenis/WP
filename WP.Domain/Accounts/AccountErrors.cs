namespace WP.Domain.Accounts;

/// <summary>
/// Define los errores de dominio relacionados con Account.
/// </summary>
public static class AccountErrors
{
    /// <summary>
    /// Error cuando no se encuentra una cuenta por su identificador.
    /// </summary>
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Account.NotFound",
            $"La cuenta con id '{id}' no fue encontrada.");

    /// <summary>
    /// Error cuando los fondos son insuficientes para realizar la operación.
    /// </summary>
    public static readonly Error InsufficientFunds =
        Error.Failure(
            "Account.InsufficientFunds",
            "Fondos insuficientes para realizar la operación.");
}
