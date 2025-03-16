using System.Security.Cryptography;
using System.Text;

namespace BloggingPLatformApi.Services;

public static class PasswordHasher
{
    public static string HashPassword(this string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes); 
    }

    public static bool VerifyPassword(this string password, string passwordHash)
    {
        var hashOfInput = password.HashPassword();
        return hashOfInput == passwordHash;
    }
}