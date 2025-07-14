namespace Cineverse.Infrastructure.Common.Constants;

internal class CacheConstants
{
    public static string VerificationCodeCacheKey(string email) => "verification_code_" + email;
    public const int VerificationCodeCacheLifetime = 2;
}