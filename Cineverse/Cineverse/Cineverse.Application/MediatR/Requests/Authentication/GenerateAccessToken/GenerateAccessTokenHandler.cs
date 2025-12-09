using Cineverse.Identity.Services.Authentication;
using Cineverse.Infrastructure.Common.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.GenerateAccessToken;

public class GenerateAccessTokenHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<GenerateAccessTokenRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(GenerateAccessTokenRequest request, CancellationToken cancellationToken)
    {
        return await authenticationService.GenerateAccessTokenAsync(request.RefreshToken);
    }
}