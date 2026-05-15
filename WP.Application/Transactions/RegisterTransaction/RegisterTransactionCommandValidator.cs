namespace WP.Application.Transactions.RegisterTransaction;

/// <summary>
/// Validador para el comando de registro de transacción.
/// </summary>
public sealed class RegisterTransactionCommandValidator
    : IValidator<RegisterTransactionCommand>
{
    private static readonly string[] _validTypes = ["Income", "Expense", "Transfer"];

    /// <summary>
    /// Valida el comando de registro de transacción.
    /// </summary>
    /// <param name="value">El comando a validar.</param>
    public void Validate(RegisterTransactionCommand value)
    {
        var errors = new Dictionary<string, List<string>>();

        if (value.AccountId == Guid.Empty)
        {
            errors.AddError("AccountId", "El identificador de la cuenta es requerido.");
        }

        if (value.Amount <= 0)
        {
            errors.AddError("Amount", "El monto debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(value.CurrencyCode))
        {
            errors.AddError("CurrencyCode", "El código de moneda es requerido.");
        }

        if (value.CurrencyCode?.Length != 3)
        {
            errors.AddError("CurrencyCode", "El código de moneda debe tener 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(value.Type))
        {
            errors.AddError("Type", "El tipo de transacción es requerido.");
        }

        if (!_validTypes.Contains(value.Type))
        {
            errors.AddError("Type", "El tipo debe ser Income, Expense o Transfer.");
        }

        if (value is { Type: "Transfer", TargetAccountId: null })
        {
            errors.AddError("TargetAccountId", "La cuenta destino es requerida para transferencias.");
        }

        if (value.Type == "Transfer" && value.TargetAccountId == value.AccountId)
        {
            errors.AddError("TargetAccountId", "La cuenta destino no puede ser la misma que la cuenta origen.");
        }

        if(errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
