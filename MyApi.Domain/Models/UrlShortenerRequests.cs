namespace MyApi.Domain.Models;

public sealed record CreateShortUrlRequest(string LongUrl);

public sealed record GetUserShortUrlsRequest(int UserId);

public sealed record CreateUrlMappingRequest(
    string LongUrl,
    string ShortUrl,
    string HashedLongUrl,
    int UserId
);

public sealed record DeleteShortUrlRequest(string ShortUrl, int UserId);

public sealed record GetShortUrlByHashedLongUrlRequest(string HashedLongUrl, int UserId);
