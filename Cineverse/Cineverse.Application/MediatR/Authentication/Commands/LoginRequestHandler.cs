using System.Net;
using Cineverse.Application.Common.Models;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;
using MongoDB.Driver.Linq;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record LoginRequest(
    string Email,
    string Password
) : IRequest<LoginResponse>;

public class LoginRequestHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IAuthenticationService authenticationService,
    IUserContext userContext
) : IRequestHandler<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByCredentialsAsync(request.Email, request.Password)
                   ?? throw new ApiRequestException("Invalid credentials, please try again", HttpStatusCode.BadRequest);

        var accessToken = authenticationService.GenerateJwtToken(
            user.Email,
            user.Role,
            user.FirstName + user.LastName
        );

        var refreshToken = await refreshTokenRepository
            .AsQueryable()
            .FirstOrDefaultAsync(
                x => x.UserId == user.Id,
                cancellationToken: cancellationToken
            );

        if (refreshToken == null)
        {
            refreshToken = new RefreshTokenEntity
            {
                UserId = user.Id,
                Token = authenticationService.GenerateRefreshToken()
            };

            await refreshTokenRepository.InsertOneAsync(refreshToken, cancellationToken);
        }

        userContext.AddRefreshTokenToCookie(refreshToken.Token);
        return new LoginResponse(refreshToken.Token, accessToken);
    }
}