using Cineverse.Infrastructure.Common.Models;
using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record LoginRequest(
    string Email,
    string Password
) : IRequest<AuthenticationResponse>;

public class LoginRequestHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<LoginRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return await authenticationService.LoginUserAsync(request.Email, request.Password);
    }
}