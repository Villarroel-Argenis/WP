namespace WP.Infrastructure.Persistence;

/// <summary>
/// Contexto de base de datos para la aplicación WP utilizando Entity Framework.
/// </summary>
public class WpDbContext(DbContextOptions options,
    OutboxInterceptor outboxInterceptor) : DbContext(options), IUnitOfWork
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
    /// Conjunto de mensajes del outbox pendientes de procesamiento.
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    /// <summary>
    /// Configura el modelo de datos al crear el modelo.
    /// </summary>
    /// <param name="modelBuilder">El constructor del modelo utilizado para configurar las entidades.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WpDbContext).Assembly);

    /// <summary>
    /// Configura el interceptor de outbox al inicializar el contexto.
    /// </summary>
    /// <param name="optionsBuilder">Constructor de opciones del contexto.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.AddInterceptors(outboxInterceptor);
}
