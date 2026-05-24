using System.Security.Cryptography;
using System.Text;

using MyApi.Domain.Interfaces;

namespace MyApi.Infrastructure.Generators;

public class HashCodeGenerator: IHashCodeGenerator
{
    public string GenerateHash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes); // Or ToBase64String
        }
    }
}