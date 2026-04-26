using Microsoft.Extensions.DependencyInjection;
using MyApi.Domain.Interfaces;
using MyApi.Domain.Services;

namespace MyApi.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        return services;
    }
}