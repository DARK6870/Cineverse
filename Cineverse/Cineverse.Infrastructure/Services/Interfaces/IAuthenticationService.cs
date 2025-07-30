using Cineverse.Infrastructure.Common.Models;

namespace Cineverse.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponse> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password
    );

    Task ConfirmUserEmailAsync(int verificationCode);

    Task GenerateVerificationCodeAsync(string email, string fullName);
    
    Task<LoginResponse> LoginUserAsync(string email, string password);

    Task LogoutUserAsync();
}