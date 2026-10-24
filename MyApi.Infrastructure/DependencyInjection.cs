using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MyApi.Domain.Interfaces;
using MyApi.Infrastructure.Repositories;
using MyApi.Infrastructure.Persistence;
using MyApi.Infrastructure.Generators;

namespace MyApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        return services
            .AddDatabaseServices(connectionString)
            .AddAuthenticationRepositories()
            .AddDomainRepositories()
            .AddCodeGenerators();
    }

    private static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        return services
            .AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
    }

    private static IServiceCollection  AddAuthenticationRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    private static IServiceCollection AddDomainRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IUrlShortenerRepository, UrlShortenerRepository>();
    }

    private static IServiceCollection AddCodeGenerators(this IServiceCollection services)
    {
        return services
            .AddScoped<IShortCodeGenerator, RandomShortCodeGenerator>()
            .AddScoped<IHashCodeGenerator, HashCodeGenerator>();
    }
}