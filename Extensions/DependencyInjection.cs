
using MyApi.Domain;
using MyApi.Infrastructure;

namespace MyApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string connectionString)
    {
        services.AddDomain();
        services.AddInfrastructure(connectionString);
        return services;
    }
}