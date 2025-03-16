namespace BloggingPLatformApi.Entities;

public class User : EntityBase
{
    public required string Username { get; set; }
    public string? PasswordHash { get; set; }
    public required string Role { get; set; }
}