using Cineverse.Infrastructure.Common.Models;

namespace Cineverse.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password
    );

    Task ConfirmUserEmailAsync(int verificationCode);

    Task GenerateVerificationCodeAsync(string email, string fullName);
    
    Task<AuthenticationResponse> LoginUserAsync(string email, string password);
    
    Task<AuthenticationResponse> GenerateAccessTokenAsync(string refreshToken);
    
    // TODO: add logic to change user password
}