namespace MyApi.Models.Responses;

/// <summary>
/// Standard error response format for all API errors
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? ErrorId { get; set; } = Guid.NewGuid().ToString("N");

    public ErrorResponse(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }
}
