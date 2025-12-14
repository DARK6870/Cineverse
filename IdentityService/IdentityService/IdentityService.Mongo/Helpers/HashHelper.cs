using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Mongo.Helpers;

internal static class HashHelper
{
    public static string ComputeHash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(inputBytes);

        var builder = new StringBuilder();
        foreach (var t in hashBytes)
            builder.Append(t.ToString("x2"));

        return builder.ToString();
    }
}