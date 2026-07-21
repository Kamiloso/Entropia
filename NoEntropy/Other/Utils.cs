using System.Security.Cryptography;
using System.Text;

namespace NoEntropy.Other;

internal static class Utils
{
    public static string HashString(string source)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(source));

        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString().ToUpperInvariant();
    }
}
