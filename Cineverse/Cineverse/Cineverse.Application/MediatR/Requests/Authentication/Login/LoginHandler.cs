using Cineverse.Identity.Services.Authentication;
using Cineverse.Infrastructure.Common.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.Login;

public class LoginHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<LoginRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return await authenticationService.LoginUserAsync(request.Email, request.Password);
    }
}