namespace MyApi.Domain.Exceptions;

/// <summary>
/// Base class for all custom application exceptions
/// Carries HTTP status code AND user-friendly message
/// Global middleware returns both to client
/// </summary>
public abstract class CustomException : Exception
{
    /// <summary>
    /// HTTP status code to return to client
    /// Derived classes override this with appropriate status
    /// </summary>
    public abstract int StatusCode { get; }

    /// <summary>
    /// User-friendly message (what to display to client)
    /// Examples: "Resource not found", "Invalid email", "Quota exceeded"
    /// </summary>
    public abstract string UserMessage { get; }

    public CustomException(string message) : base(message) { }
    
    public CustomException(string message, Exception innerException) 
        : base(message, innerException) { }
}