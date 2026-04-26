namespace MyApi.Middleware;

/// <summary>
/// Extension methods for registering middlewares
/// Centralized location for all middleware registrations
/// Add new middleware extension methods here as they're created
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Registers the global exception handling middleware
    /// Should be called early in the middleware pipeline
    /// </summary>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandler>();
    }
}
