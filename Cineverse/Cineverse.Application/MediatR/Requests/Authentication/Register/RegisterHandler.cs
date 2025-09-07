using Cineverse.Identity.Services.Authentication;
using Cineverse.Infrastructure.Common.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.Register;

public class RegisterHandler(
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