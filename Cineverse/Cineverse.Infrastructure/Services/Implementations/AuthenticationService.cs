using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Infrastructure.Common.Models;
using Cineverse.Infrastructure.Common.Models.Options;
using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Enums;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Cineverse.Infrastructure.Services.Implementations;

public class AuthenticationService(
    AuthenticationOptions authenticationOptions,
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUserContext userContext,
    IMemoryCache cache,
    INotificationService notificationService
) : IAuthenticationService
{
    public async Task<LoginResponse> RegisterUserAsync(
        string email,
        string firstName,
        string lastName,
        string password
    )
    {
        if (await userRepository.ExistsAsync(x => x.Email == email))
            throw new ApiRequestException("This email already exists", HttpStatusCode.Conflict);
        
        // Register a new user
        var user = new UserEntity
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
        
        await userRepository.CreateUserAsync(user, password);

        // Generate verification code
        await GenerateVerificationCodeAsync(email, user.FirstName + ' ' + user.LastName);

        // Generate refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = user.Id,
            Token = refreshToken
        };

        await refreshTokenRepository.InsertOneAsync(refreshTokenEntity);
        userContext.AddRefreshTokenToCookie(refreshToken);

        // Generate access token
        var accessToken = GenerateJwtToken(user);
        return new LoginResponse(refreshToken, accessToken);
    }

    public async Task ConfirmUserEmailAsync(int verificationCode)
    {
        var user = await userRepository
                       .FindByIdAsync(userContext.UserId)
                   ?? throw new ApiRequestException("User not found", HttpStatusCode.NotFound);

        if (user.Status is not UserStatus.PendingEmailConfirmation)
            throw new ApiRequestException("Email already confirmed", HttpStatusCode.Conflict);

        if (!cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(user.Email), out var cachedVerificationCode) || verificationCode != cachedVerificationCode)
            throw new ApiRequestException("Verification code invalid or expired, please try again", HttpStatusCode.BadRequest);

        // Set user status to normal
        user.Status = UserStatus.Normal;
        await userRepository.ReplaceOneAsync(user);
    }

    public async Task GenerateVerificationCodeAsync(string email, string fullName)
    {
        var random = new Random();
        var verificationCode = random.Next(11111, 99999);

        if (cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheLifetime, out var _))
            throw new ApiRequestException("You already received verification code, try again later", HttpStatusCode.BadRequest);
        
        cache.Set(
            CacheConstants.VerificationCodeCacheKey(email),
            verificationCode,
            TimeSpan.FromMinutes(CacheConstants.VerificationCodeCacheLifetime)
        );

        var notification = new MessageBuilder
        {
            Title = "Email Confirmation Code",
            FullName = fullName,
            Message = "Please complete your account setup to explore our website without restrictions<br><br>" +
                      $"Your verification code is <b>{verificationCode}</b><br>" +
                      $"<small>The code will be valid for {CacheConstants.VerificationCodeCacheLifetime} minutes</small>",
            ActionUrl = "https://localhost/confirm/" + verificationCode,
            ActionText = "to confirm your email"
        };
        await notificationService.SendEmailNotification(email, notification.Title, notification);
    }

    public async Task<LoginResponse> LoginUserAsync(string email, string password)
    {
        var user = await userRepository.GetUserByCredentialsAsync(email, password)
                   ?? throw new ApiRequestException("Invalid credentials, please try again", HttpStatusCode.BadRequest);

        var refreshTokenEntity = refreshTokenRepository
            .AsQueryable()
            .FirstOrDefault(x => x.UserId == user.Id);

        if (refreshTokenEntity is null)
        {
            refreshTokenEntity = new RefreshTokenEntity
            {
                UserId = user.Id,
                Token = GenerateRefreshToken()
            };
            
            await refreshTokenRepository.InsertOneAsync(refreshTokenEntity);
        }
        
        userContext.AddRefreshTokenToCookie(refreshTokenEntity.Token);
        var accessToken = GenerateJwtToken(user);

        return new LoginResponse(refreshTokenEntity.Token, accessToken);
    }

    public async Task LogoutUserAsync()
    {
        var cookieRefreshToken = userContext.GetRefreshTokenFromCookie();
        if (cookieRefreshToken is null)
            return;
        
        await refreshTokenRepository.DeleteOneAsync(x => x.Token == cookieRefreshToken);
        userContext.RemoveRefreshTokenFromCookie();
    }

    private string GenerateRefreshToken()
    {
        const int tokenSize = 32;

        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[tokenSize];
        rng.GetBytes(randomBytes);
        
        return Convert.ToBase64String(randomBytes);
    }
    
    private string GenerateJwtToken(UserEntity user)
    {
        var jwtOptions = authenticationOptions.JwtOptions;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtOptions.SecretKey);

        var claims = new List<Claim>
        {
            new(JwtClaims.UserIdClaimType, user.Id),
            new(ClaimTypes.Name, user.FirstName + ' ' + user.LastName),
            new(JwtClaims.UserStatusClaimType, user.Status.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpireInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}