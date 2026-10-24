using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.Infrastructure.Entities;

[Table("users")]
public class UserEntity
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string GoogleId { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
}