namespace Cineverse.Identity.Services.RestorePassword;

public interface IRestorePasswordService
{
    Task GenerateAndSendPasswordResetCode(string email);

    Task<bool> RestorePasswordByCode(string email, string code, string password);
}