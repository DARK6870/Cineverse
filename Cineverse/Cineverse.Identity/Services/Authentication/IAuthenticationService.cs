using Cineverse.Infrastructure.Common.Models;

namespace Cineverse.Identity.Services.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password
    );

    Task ConfirmUserEmailAsync(int verificationCode);

    Task<AuthenticationResponse> LoginUserAsync(string email, string password);
    
    Task<AuthenticationResponse> GenerateAccessTokenAsync(string refreshToken);
    
    // TODO: add logic to change user password
}