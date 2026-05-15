namespace WP.Application;

/// <summary>
/// Proporciona métodos de extensión para configurar la inyección de dependencias en la capa de aplicación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega los servicios de la capa de aplicación al contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios a la que se agregarán los servicios de aplicación.</param>
    /// <returns>La colección de servicios actualizada con los servicios de aplicación agregados.</returns>
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateAccountCommand, Guid>, CreateAccountCommandHandler>();
        services.AddScoped<IQueryHandler<GetAccountByIdQuery, AccountResponse?>, GetAccountByIdQueryHandler>();

        services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
        return services;
    }
}
