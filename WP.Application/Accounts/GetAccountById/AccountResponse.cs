namespace WP.Application.Accounts.GetAccountById;

/// <summary>
/// Respuesta que contiene los datos de una cuenta.
/// </summary>
/// <param name="Id">El identificador único de la cuenta.</param>
/// <param name="Name">El nombre de la cuenta.</param>
/// <param name="Amount">El monto o saldo de la cuenta.</param>
/// <param name="CurrencyCode">El código ISO de la moneda de la cuenta.</param>
/// <param name="CreatedAt">La fecha y hora de creación de la cuenta.</param>
public sealed record AccountResponse(
    Guid Id,
    string Name,
    decimal Amount,
    string CurrencyCode,
    DateTime CreatedAt);
