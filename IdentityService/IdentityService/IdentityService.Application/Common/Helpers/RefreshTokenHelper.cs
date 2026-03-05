using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityService.Application.Common.Helpers;

public static class RefreshTokenHelper
{
    private const int RefreshTokenSize = 64;

    public static string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(RefreshTokenSize);
        return WebEncoders.Base64UrlEncode(bytes);
    }
}