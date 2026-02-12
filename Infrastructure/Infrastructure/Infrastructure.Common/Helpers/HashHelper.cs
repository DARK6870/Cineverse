using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Common.Helpers;

public static class HashHelper
{
    public static string ComputeSha256(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}