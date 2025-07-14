using Cineverse.Infrastructure.Common.Models;

namespace Cineverse.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    Task RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password
    );

    Task ConfirmUserEmailAsync(string email, string verificationCode);

    Task GenerateVerificationCodeAsync(string email);
    
    Task<LoginResponse> LoginUserAsync(string email, string password);

    Task LogoutUserAsync();
}