using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BloggingPLatformApi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BloggingPLatformApi.Services;

public static class TokenGenerator
{
    public static string GenerateToken(this User user, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expireMinutes = int.Parse(jwtSettings["ExpireMinutes"] ?? "30");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}