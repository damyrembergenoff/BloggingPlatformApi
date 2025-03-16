using System.Security.Claims;
using BloggingPLatformApi.Abstractions;
using BloggingPLatformApi.Entities;

namespace BloggingPLatformApi.Services;

public class AuthenticationStateService(IHttpContextAccessor httpContextAccessor) : IAuthenticationStateService
{
    private ClaimsPrincipal? user => httpContextAccessor?.HttpContext?.User;

    public bool IsAuthenticated => user?.Identity?.IsAuthenticated is true;

    public string? UserId => (user?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)))?.Value;

    public string? UserName => (user?.Claims?.FirstOrDefault(c => c.Type.Equals("username")))?.Value;

    public string? Role => (user?.Claims?.FirstOrDefault(c => c.Type.Equals("role")))?.Value;
    public User? User
    {
        get
        {
            if(IsAuthenticated is false || UserId is null)
                return null;

            User user = new()
            {
                Id = Guid.Parse(UserId),
                Username = UserName!,
                Role = Role!
            };
            
            return user;
        }
    }
}
