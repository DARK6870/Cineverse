namespace IdentityService.Application.Services.Password;

public interface IRestorePasswordService
{
    /// <summary>
    /// Generate and send restore password email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task GenerateAndSendPasswordResetCode(string email);

    /// <summary>
    /// Restore password using code
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> RestorePasswordByCode(string email, string code, string password);
}