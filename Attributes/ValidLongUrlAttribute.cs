using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MyApi.Attributes
{
    /// <summary>
    /// Validates that a long URL is safe and properly formatted.
    /// Rules:
    /// - Required
    /// - Must be a valid absolute URL
    /// - Only http/https allowed
    /// - Max length restriction
    /// - Blocks localhost and private IPs
    /// </summary>
    public class ValidLongUrlAttribute : ValidationAttribute
    {
        private const int MaxLength = 2000;

        public ValidLongUrlAttribute()
            : base("Invalid URL format")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                ErrorMessage = "URL is required";
                return false;
            }

            var url = value.ToString()!.Trim();

            // Length check
            if (url.Length > MaxLength)
            {
                ErrorMessage = $"URL must not exceed {MaxLength} characters";
                return false;
            }

            // Try to create URI
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                ErrorMessage = "Invalid URL format";
                return false;
            }

            // Allow only HTTP/HTTPS
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                ErrorMessage = "Only HTTP and HTTPS URLs are allowed";
                return false;
            }

            // Block localhost
            if (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                ErrorMessage = "Localhost URLs are not allowed";
                return false;
            }

            // Block private IPs
            if (IPAddress.TryParse(uri.Host, out var ip))
            {
                if (IsPrivateIp(ip))
                {
                    ErrorMessage = "Private or internal IPs are not allowed";
                    return false;
                }
            }

            return true;
        }

        private bool IsPrivateIp(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();

            return ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                   (
                       bytes[0] == 10 ||                                  // 10.x.x.x
                       (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) || // 172.16–31.x.x
                       (bytes[0] == 192 && bytes[1] == 168)              // 192.168.x.x
                   );
        }
    }
}