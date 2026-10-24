
using MyApi.Domain;
using MyApi.Infrastructure;
using MyApi.Services;

namespace MyApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddDomain();
        services.AddInfrastructure(connectionString);
        return services;
    }
}