using System.Net;
using Auth.Models.Responses;
using IdentityService.Application.Services.Token;
using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.GenerateAccessToken;

public class GenerateAccessTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IUserContext userContext,
    ITokenService tokenService
) : IRequestHandler<GenerateAccessTokenRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(GenerateAccessTokenRequest request, CancellationToken cancellationToken)
    {
        var token = await refreshTokenRepository.GetRefreshTokenAsync(
            request.RefreshToken,
            userContext.IpAddress
        ) ??  throw new ApiRequestException("No active sessions found", HttpStatusCode.Unauthorized);

        var user = await userRepository.FindByIdOrThrowAsync(token.UserId, cancellationToken);
        var accessToken = tokenService.GenerateJwtToken(user);
        
        return new AuthenticationResponse(request.RefreshToken, accessToken);
    }
}