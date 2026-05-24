using System.Security.Cryptography;

using MyApi.Domain.Interfaces;
namespace MyApi.Infrastructure.Generators;

public class RandomShortCodeGenerator: IShortCodeGenerator
{
    public string Generate()  
    {  
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  
        var bytes = new byte[8];  
        RandomNumberGenerator.Fill(bytes);  
    
       return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
}