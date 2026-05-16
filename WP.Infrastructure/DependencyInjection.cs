namespace WP.Infrastructure;

/// <summary>
/// Proporciona métodos de extensión para configurar la inyección de dependencias en la capa de infraestructura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega los servicios de infraestructura al contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios a la que se agregarán los servicios de infraestructura.</param>
    /// <param name="configuration">La configuración de la aplicación utilizada para configurar los servicios.</param>
    /// <returns>La colección de servicios actualizada con los servicios de infraestructura agregados.</returns>
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WpDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<WpDbContext>());

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IDomainEventConsumer<TransactionRegisteredDomainEvent>,
            OnTransactionRegistered>();

        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        return services;
    }
}
