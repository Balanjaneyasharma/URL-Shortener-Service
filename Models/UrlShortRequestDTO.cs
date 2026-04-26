using MyApi.Attributes;

namespace MyApi.Models;

public class UrlShortRequestDTO
{
    [ValidLongUrl]
    public required string LongUrl { get; set; }
}