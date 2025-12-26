using System.Net;
using Auth.Models.Responses;
using IdentityService.Application.Services.RefreshToken;
using IdentityService.Application.Services.Token;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.Login;

public class LoginHandler(
    IUserRepository userRepository,
    IRefreshTokenService refreshTokenService,
    IUserContext userContext,
    ITokenService tokenService
) : IRequestHandler<LoginRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByCredentialsAsync(request.Email, request.Password)
                   ?? throw new ApiRequestException("Invalid credentials, please try again", HttpStatusCode.BadRequest);

        var refreshToken = await refreshTokenService.CreateOrUpdateTokenAsync(
            user.Id,
            userContext.IpAddress
        );
        
        var accessToken = tokenService.GenerateJwtToken(user);

        return new AuthenticationResponse(refreshToken, accessToken);
    }
}