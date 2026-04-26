using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MyApi.Domain.Interfaces;
using MyApi.Infrastructure.Repositories;
using MyApi.Infrastructure.Persistence;

namespace MyApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IUrlShortenerRepository, UrlShortenerRepository>();
        services.AddScoped<IShortCodeGenerator, RandomShortCodeGenerator>();
        
        return services;
    }
}