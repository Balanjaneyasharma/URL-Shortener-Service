using System.Text.Json;
using MyApi.Domain.Exceptions;
using MyApi.Models.Responses;

namespace MyApi.Middleware;

/// <summary>
/// Global exception handling middleware
/// Simple and scalable:
/// - If CustomException: return its StatusCode + UserMessage
/// - Else: return 500 Internal Server Error
/// </summary>
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ErrorResponse response;

        if (exception is CustomException customEx)
        {
            // Custom exception: use its status code and user-friendly message
            response = new ErrorResponse(
                customEx.StatusCode,
                customEx.UserMessage,
                customEx.Message  // Technical details (not shown to user in production)
            );
        }
        else
        {
            // Unexpected error: return 500
            response = new ErrorResponse(
                500,
                "Internal server error",
                "An unexpected error occurred. Please contact support if this persists."
            );
        }

        context.Response.StatusCode = response.StatusCode;
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
