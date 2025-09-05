namespace Cineverse.Identity.Common.Constants;

internal class CacheConstants
{
    public static string VerificationCodeCacheKey(string email) => "verification_code_" + email;
    public const int VerificationCodeCacheLifetimeMinutes = 2;
    // TODO: check if code was already generated
}