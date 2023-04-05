using System.Security.Cryptography;
using System.Text;

namespace Employee.Services;

public class PasswordHasher
{
    // Hash password method 
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    // Verify password method
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var newHash = HashPassword(password);
        return newHash == hashedPassword;
    }
}