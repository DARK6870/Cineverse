namespace IdentityService.Application.Services.EmailVerification;

public interface IEmailVerificationService
{
    /// <summary>
    /// Generate and send email verification code to user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    Task<int> GenerateAndSendVerificationCodeAsync(string email, string fullName);
    
    /// <summary>
    /// Validate verification code
    /// </summary>
    /// <param name="email"></param>
    /// <param name="verificationCode"></param>
    /// <returns></returns>
    Task<bool> ValidateVerificationCodeAsync(string email, int verificationCode);

    /// <summary>
    /// Verify email
    /// </summary>
    /// <param name="verificationCode"></param>
    /// <returns></returns>
    Task ConfirmEmailAsync(int verificationCode);
}