namespace IdentityService.Application.Constants;

public static class CacheConstants
{
    public static string VerificationCodeCacheKey(string email) => "verification_code_" + email;
    public const int VerificationCodeCacheLifetimeMinutes = 2;
    
    public static string RestorePasswordCacheKey(string email) => "restore_password_" + email;
    public const int RestorePasswordCacheLifetimeMinutes = 10;
}