using System.Net;
using Auth.Models.Enums;
using Auth.Models.Responses;
using IdentityService.Application.Services.EmailVerification;
using IdentityService.Application.Services.RefreshToken;
using IdentityService.Application.Services.Token;
using IdentityService.Mongo.Repositories.User;
using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.Register;

public class RegisterHandler(
    IUserRepository userRepository,
    IEmailVerificationService emailVerificationService,
    ITokenService tokenService,
    IRefreshTokenService refreshTokenService,
    IUserContext userContext
) : IRequestHandler<RegisterRequest, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(x => x.Email == request.Email, cancellationToken))
            throw new ApiRequestException("This email already exists", HttpStatusCode.Conflict);

        var user = new UserEntity
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Status = UserStatus.PendingEmailConfirmation
        };
        await userRepository.CreateUserAsync(user, request.Password);

        await emailVerificationService.GenerateAndSendVerificationCodeAsync(user.Email, user.GetFullName());

        var refreshToken = await refreshTokenService.CreateOrUpdateTokenAsync(
            user.Id,
            userContext.IpAddress
        );
        
        var accessToken = tokenService.GenerateJwtToken(user);
        
        return new AuthenticationResponse(refreshToken, accessToken);
    }
}