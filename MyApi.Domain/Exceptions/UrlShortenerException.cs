namespace MyApi.Domain.Exceptions;

/// <summary>
/// Base exception for all URL Shortener domain logic errors
/// All domain exceptions should inherit from this
/// </summary>
public class UrlShortenerException : CustomException
{
    public override int StatusCode => 400;
    public override string UserMessage => "Invalid request";

    public UrlShortenerException(string message) : base(message) { }
    public UrlShortenerException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Thrown when a short URL is not found in the database
/// Returns 404 Not Found
/// </summary>
public class UrlNotFoundException : UrlShortenerException
{
    public override int StatusCode => 404;
    public override string UserMessage => "Short URL not found";

    public UrlNotFoundException(string shortUrl) 
        : base($"Short URL '{shortUrl}' not found") { }
}

/// <summary>
/// Thrown when attempting to create a duplicate short URL
/// Returns 409 Conflict
/// </summary>
public class UrlAlreadyShortenedException : UrlShortenerException
{
    public override int StatusCode => 409;
    public override string UserMessage => "URL is already shortened";

    public UrlAlreadyShortenedException(string longUrl, string existingShortUrl) 
        : base($"The long URL '{longUrl}' already has a short URL assigned: '{existingShortUrl}'") { }
}

/// <summary>
/// Thrown when tried to store short ulr that is already in db
/// this will be internally handled
/// </summary>
public class DuplicateShortCodeException: UrlShortenerException
{
    public override int StatusCode => 409;
    public override string UserMessage => "Short code already exists";
    public DuplicateShortCodeException(string code)
        : base($"Short code '{code}' already exists.") {}
}

/// <summary>
/// Thrown after max  tries of creating short url with given long url 
/// </summary>
public class ShortCodeGenerationFailedException: UrlShortenerException
{
    public override int StatusCode => 500;
    public override string UserMessage => "Short Url creation failed";

    public ShortCodeGenerationFailedException(string message): base(message) {}
}

/// <summary>
/// Thrown when the provided URL format is invalid
/// Returns 422 Unprocessable Entity
/// </summary>
public class InvalidUrlException : UrlShortenerException
{
    public override int StatusCode => 422;
    public override string UserMessage => "Invalid URL format";

    public InvalidUrlException(string url) 
        : base($"The URL '{url}' is not in a valid format") { }
}


