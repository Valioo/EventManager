using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Domain.Helper;


public static class PasswordHashing
{
    public static string HashPassword(string password)
    {
        // Generate a 16-byte salt
        var salt = RandomNumberGenerator.GetBytes(16);

        // Derive hash using PBKDF2 (SHA-256)
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32); // 256-bit

        // Store as Base64(salt).Base64(hash)
        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split('.');

        if (parts.Length != 2)
            return false;

        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = Convert.FromBase64String(parts[1]);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        var computedHash = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}
