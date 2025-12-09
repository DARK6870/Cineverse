namespace Cineverse.Identity.Services.EmailVerification;

public interface IVerificationService
{
    Task<int> GenerateAndSendVerificationCodeAsync(string email, string fullName);
    
    Task<bool> ValidateVerificationCodeAsync(string email, int verificationCode);
}