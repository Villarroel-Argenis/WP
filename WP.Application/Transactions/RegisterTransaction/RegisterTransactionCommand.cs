namespace WP.Application.Transactions.RegisterTransaction;


/// <summary>
/// Comando para registrar una nueva transacción en el sistema.
/// </summary>
/// <param name="AccountId">Identificador de la cuenta origen.</param>
/// <param name="Amount">Monto de la transacción.</param>
/// <param name="CurrencyCode">Código de la moneda.</param>
/// <param name="Type">Tipo de transacción: Income, Expense o Transfer.</param>
/// <param name="Description">Descripción opcional de la transacción.</param>
/// <param name="TargetAccountId">Identificador de la cuenta destino, solo para transferencias.</param>
/// <param name="Tags">Etiquetas opcionales de la transacción.</param>
public sealed record RegisterTransactionCommand(
    Guid AccountId,
    decimal Amount,
    string CurrencyCode,
    string Type,
    string? Description = null,
    Guid? TargetAccountId = null,
    IEnumerable<string>? Tags = null) : ICommand<Guid>;
