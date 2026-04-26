namespace MyApi.Domain.Models;

public class UrlMapping
{
    public int Id { get; set; }

    public required string LongUrl { get; set; }

    public required string ShortUrl { get; set; }
}