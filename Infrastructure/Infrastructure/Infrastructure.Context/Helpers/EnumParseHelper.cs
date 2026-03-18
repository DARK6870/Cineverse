using System.Security.Claims;

namespace Infrastructure.Context.Helpers;

internal static class EnumParseHelper
{
    public static T ParseEnumClaim<T>(ClaimsPrincipal user, string claimType, T defaultValue) where T : struct
    {
        var claimValue = user.FindFirst(claimType)?.Value;
        return !string.IsNullOrEmpty(claimValue) && Enum.TryParse<T>(claimValue, true, out var result)
            ? result
            : defaultValue;
    }
}