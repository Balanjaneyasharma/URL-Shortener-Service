namespace MyApi.Domain.Interfaces;

public interface IHashCodeGenerator
{
    public string GenerateHash(string input);
}