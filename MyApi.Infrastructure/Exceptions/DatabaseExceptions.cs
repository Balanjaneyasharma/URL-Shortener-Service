using MyApi.Domain.Exceptions;

namespace MyApi.Infrastructure.Exceptions;

/// <summary>
/// Thrown when database operations timeout
/// Returns 503 Service Unavailable
/// Client should retry after a delay
/// </summary>
public class DatabaseTimeoutException : CustomException
{
    public override int StatusCode => 503;
    public override string UserMessage => "Service temporarily unavailable";

    public DatabaseTimeoutException(string operation) 
        : base($"Database operation '{operation}' timed out. Please try again later") { }
}

/// <summary>
/// Thrown when database connection fails
/// Returns 503 Service Unavailable
/// Indicates persistent database connectivity problems
/// </summary>
public class DatabaseConnectionException : CustomException
{
    public override int StatusCode => 503;
    public override string UserMessage => "Database connection failed";

    public DatabaseConnectionException() 
        : base("Unable to connect to the database. Service temporarily unavailable") { }
}
