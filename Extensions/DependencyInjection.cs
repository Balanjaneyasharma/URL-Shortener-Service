
using MyApi.Domain.Interfaces;
using MyApi.Domain;
using MyApi.Infrastructure;
using MyApi.Services;

namespace MyApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string connectionString)
    {
        return services
                .AddAuthServices()
                .AddDomain()
                .AddInfrastructure(connectionString);
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        return services
            .AddHttpContextAccessor()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IJwtService, JwtService>();
    }
}