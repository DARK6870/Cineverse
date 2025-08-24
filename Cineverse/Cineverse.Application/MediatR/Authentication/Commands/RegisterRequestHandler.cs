using Cineverse.Infrastructure.Common.Models;
using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
) : IRequest<AuthenticationResponse>;

public class RegisterRequestHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<RegisterRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        return await authenticationService.RegisterUserAsync(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password
        );
    }
}