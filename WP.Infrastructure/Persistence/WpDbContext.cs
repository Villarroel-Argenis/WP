namespace WP.Infrastructure.Persistence;

/// <summary>
/// Contexto de base de datos para la aplicación WP utilizando Entity Framework.
/// </summary>
public class WpDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Obtiene el conjunto de entidades Account.
    /// </summary>
    public DbSet<Account> Accounts => Set<Account>();

    /// <summary>
    /// Obtiene el conjunto de entidades Transaction.
    /// </summary>
    public DbSet<Transaction> Transactions => Set<Transaction>();

    /// <summary>
    /// Configura el modelo de datos al crear el modelo.
    /// </summary>
    /// <param name="modelBuilder">El constructor del modelo utilizado para configurar las entidades.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WpDbContext).Assembly);

}
