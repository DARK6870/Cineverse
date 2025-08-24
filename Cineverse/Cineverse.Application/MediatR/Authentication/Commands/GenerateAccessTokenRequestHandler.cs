using Cineverse.Infrastructure.Common.Models;
using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record GenerateAccessTokenRequest(string RefreshToken) : IRequest<AuthenticationResponse>;

public class GenerateAccessTokenRequestHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<GenerateAccessTokenRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(GenerateAccessTokenRequest request, CancellationToken cancellationToken)
    {
        return await authenticationService.GenerateAccessTokenAsync(request.RefreshToken);
    }
}