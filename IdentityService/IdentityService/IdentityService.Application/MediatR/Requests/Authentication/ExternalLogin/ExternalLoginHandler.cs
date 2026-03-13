using System.Security.Claims;
using Auth.Models.Enums;
using IdentityService.Application.Services.RefreshToken;
using IdentityService.Application.Services.Token;
using IdentityService.Mongo.Repositories.User;
using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Common.Exceptions;
using Infrastructure.Context.UserContext;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Application.MediatR.Requests.Authentication.ExternalLogin;

public class ExternalLoginHandler(
    IUserRepository userRepository,
    IRefreshTokenService refreshTokenService,
    ITokenService tokenService,
    IUserContext userContext,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<ExternalLoginRequest>
{
    public async Task Handle(ExternalLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await ExtractUserAsync(request);
        
        var existingUser = await userRepository
            .FindFirstAsync(
                x => x.Email == user.Email,
                cancellationToken: cancellationToken
            );

        if (existingUser is null)
        {
            await userRepository.InsertOneAsync(user, cancellationToken);
            existingUser = user;
        }

        var refreshToken = await refreshTokenService.CreateOrUpdateTokenAsync(
            existingUser.Id,
            userContext.IpAddress
        );
        
        var accessToken = tokenService.GenerateJwtToken(existingUser);

        AppendTokensToResponse(refreshToken, accessToken);
    }

    private async Task<UserEntity> ExtractUserAsync(ExternalLoginRequest request)
    {
        var authenticateResult = await httpContextAccessor.HttpContext!.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            throw new ForbiddenException("Authentication failed");
        
        var externalUser = authenticateResult.Principal;
        
        var email = externalUser!.FindFirstValue(ClaimTypes.Email)!;
        var name = externalUser!.FindFirstValue(ClaimTypes.Name);
        var firstName = name?.Split(' ').First() ?? nameof(request.Provider);
        var lastName = name?.Split(' ').Last() ?? string.Empty;

        return new UserEntity
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Provider = request.Provider,
            Status = UserStatus.Normal
        };
    }

    private void AppendTokensToResponse(string refreshToken, string accessToken)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(
            "access_token",
            accessToken,
            new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

        httpContextAccessor.HttpContext?.Response.Cookies.Append(
            "refresh_token",
            refreshToken,
            new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
    }
}