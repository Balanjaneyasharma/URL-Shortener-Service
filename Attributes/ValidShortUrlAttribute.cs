using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyApi.Attributes;

/// <summary>
/// Validates that a short URL is properly formatted.
/// Rules:
/// - Required (not null or empty)
/// - Length between 1 and 50 characters
/// - Only alphanumeric characters, hyphens, and underscores
/// </summary>
public class ValidShortUrlAttribute : ValidationAttribute
{
    private const int MinLength = 3;
    private const int MaxLength = 50;
    private const string Pattern = @"^[a-zA-Z0-9_-]+$";

    public ValidShortUrlAttribute() 
        : base("Short URL must be 1-50 characters and contain only letters, numbers, hyphens, and underscores")
    {
    }

    public override bool IsValid(object? value)
    {
        // Check if value is null or empty
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            ErrorMessage = "Short URL is required";
            return false;
        }

        var shortUrl = value.ToString()!;
        // Check length
        if (shortUrl.Length < MinLength || shortUrl.Length > MaxLength)
        {
            ErrorMessage = $"Short URL must be between {MinLength} and {MaxLength} characters (current: {shortUrl.Length})";
            return false;
        }

        // Check pattern (alphanumeric, hyphen, underscore only)
        if (!Regex.IsMatch(shortUrl, Pattern))
        {
            ErrorMessage = "Short URL can only contain letters, numbers, hyphens, and underscores";
            return false;
        }

        return true;
    }
}
