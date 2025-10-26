using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Services.EmailVerification;
using Cineverse.Identity.Services.RefreshToken;
using Cineverse.Identity.Services.TokenManagament;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Infrastructure.Common.Models;
using Cineverse.Mongo.Repositories.User;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Enums;

namespace Cineverse.Identity.Services.Authentication;

public class AuthenticationService(
    IUserRepository userRepository,
    ITokenManagementService tokenManagementService,
    IRefreshTokenService refreshTokenService,
    IVerificationService verificationService,
    IUserContext userContext
) : IAuthenticationService
{
    public async Task<AuthenticationResponse> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password
    )
    {
        if (await userRepository.ExistsAsync(x => x.Email == email))
            throw new ApiRequestException("This email already exists", HttpStatusCode.Conflict);

        var user = new UserEntity
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Status = UserStatus.PendingEmailConfirmation
        };
        await userRepository.CreateUserAsync(user, password);

        await verificationService.GenerateAndSendVerificationCodeAsync(email, $"{firstName} {lastName}");

        var refreshTokenEntity = await refreshTokenService.CreateOrUpdateTokenAsync(
            user.Id,
            userContext.IpAddress
        );
        
        var accessToken = tokenManagementService.GenerateJwtToken(user);
        
        return new AuthenticationResponse(refreshTokenEntity.Token, accessToken);
    }

    public async Task ConfirmUserEmailAsync(int verificationCode)
    {
        var user = await userRepository.FindByIdOrThrowAsync(userContext.UserId);

        if (user.Status is not UserStatus.PendingEmailConfirmation)
            throw new ApiRequestException("Email already confirmed", HttpStatusCode.Conflict);

        if (!await verificationService.ValidateVerificationCodeAsync(user.Email, verificationCode))
            throw new ApiRequestException("Verification code invalid or expired, please try again", HttpStatusCode.BadRequest);

        await userRepository.UpdateUserStatusAsync(user.Id, UserStatus.Normal);
    }

    public async Task<AuthenticationResponse> LoginUserAsync(
        string email,
        string password
    )
    {
        var user = await userRepository.GetUserByCredentialsAsync(email, password)
                   ?? throw new ApiRequestException("Invalid credentials, please try again", HttpStatusCode.BadRequest);

        var refreshTokenEntity = await refreshTokenService.CreateOrUpdateTokenAsync(
            user.Id,
            userContext.IpAddress
        );
        
        var accessToken = tokenManagementService.GenerateJwtToken(user);

        return new AuthenticationResponse(refreshTokenEntity.Token, accessToken);
    }

    public async Task<AuthenticationResponse> GenerateAccessTokenAsync(string refreshToken)
    {
        var token = await refreshTokenService.GetRefreshTokenAsync(
            refreshToken,
            userContext.IpAddress
        ) ??  throw new ApiRequestException("No active sessions found", HttpStatusCode.BadRequest);

        var user = await userRepository.FindByIdOrThrowAsync(token.UserId);
        var accessToken = tokenManagementService.GenerateJwtToken(user);
        
        return new AuthenticationResponse(refreshToken, accessToken);
    }
}