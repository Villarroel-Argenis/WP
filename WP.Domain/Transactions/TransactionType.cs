namespace WP.Domain.Transactions;

/// <summary>
/// Define los tipos de transacción disponibles.
/// </summary>
public enum TransactionType
{
    /// <summary>Ingreso de dinero a la cuenta.</summary>
    Income,
    /// <summary>Gasto de dinero de la cuenta.</summary>
    Expense,
    /// <summary>Transferencia entre cuentas.</summary>
    Transfer
}
