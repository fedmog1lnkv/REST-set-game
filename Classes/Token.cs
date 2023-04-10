using System.Security.Cryptography;
using System.Text;

public static class TokenGenerator
{
    public static string GenerateToken(int length = 32)
    {
        var tokenData = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenData);
        }

        return Convert.ToBase64String(tokenData);
    }
}