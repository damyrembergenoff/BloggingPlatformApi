using BloggingPLatformApi.Entities;

namespace BloggingPLatformApi.Abstractions;

public interface IAuthenticationStateService
{
    User? User { get; }
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? UserName { get; }
    string? Role { get; }
}