
namespace WP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateAccountCommand, Guid>, CreateAccountCommandHandler>();
        return services;
    }
}
