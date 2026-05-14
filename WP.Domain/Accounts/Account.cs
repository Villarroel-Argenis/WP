namespace WP.Domain.Accounts;

/// <summary>
/// Representa una cuenta con un identificador único, nombre, saldo y fecha de creación.
/// </summary>
public sealed class Account
{
    /// <summary>
    /// Obtiene el identificador único de la cuenta.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Obtiene o establece el nombre de la cuenta.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Obtiene o establece el saldo de la cuenta.
    /// </summary>
    public Money Balance { get; private set; }

    /// <summary>
    /// Obtiene la fecha de creación de la cuenta en UTC.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Account.
    /// </summary>
    /// <param name="id">El identificador único de la cuenta.</param>
    /// <param name="name">El nombre de la cuenta.</param>
    /// <param name="balance">El saldo inicial de la cuenta.</param>
    /// <param name="createdAt">La fecha de creación de la cuenta.</param>
    private Account(Guid id, string name, Money balance, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Balance = balance;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Crea una nueva instancia de Account con un identificador único generado y la fecha actual.
    /// </summary>
    /// <param name="name">El nombre de la cuenta.</param>
    /// <param name="initialBalance">El saldo inicial de la cuenta.</param>
    /// <returns>Una nueva instancia de Account.</returns>
    /// <exception cref="ArgumentException">Se lanza si el nombre es nulo, vacío o solo contiene espacios en blanco.</exception>
    public static Account Create(string name, Money initialBalance)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la cuenta no puede ser vacio.", nameof(name));
        }

        var id = Guid.NewGuid();
        DateTime createdAt = DateTime.UtcNow;

        return new Account(id, name, initialBalance, createdAt);
    }
}
