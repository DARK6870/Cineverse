using System.Net;
using Auth.Models.Enums;
using IdentityService.Application.Common.Constants;
using IdentityService.Application.Common.Helpers;
using IdentityService.Application.Notifications.NotificationClientExtensions;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Common.Exceptions;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NotificationService.Client.Models.Options;
using NotificationService.Client.Services;

namespace IdentityService.Application.Services.EmailVerification;

public class EmailVerificationService(
    IUserRepository userRepository,
    IUserContext userContext,
    IMemoryCache cache,
    INotificationServiceClient notificationServiceClient,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IEmailVerificationService
{
    public async Task<int> GenerateAndSendVerificationCodeAsync(string email, string fullName)
    {
        if (cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var _))
            throw new ConflictException("You already received verification code, try again later");

        var verificationCode = EmailVerificationHelper.GenerateVerificationCode();
        
        cache.Set(
            CacheConstants.VerificationCodeCacheKey(email),
            verificationCode,
            TimeSpan.FromMinutes(CacheConstants.VerificationCodeCacheLifetimeMinutes)
        );
        
        var actionUrl = notificationLinksOptions.Value.BuildBookingDetailsUrl(email);
        await notificationServiceClient.SendVerificationEmailAsync(
            email,
            fullName,
            verificationCode,
            actionUrl
        );
        
        return verificationCode;
    }

    public Task<bool> ValidateVerificationCodeAsync(string email, int verificationCode)
    {
        if (!cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var cachedCode))
            return Task.FromResult(false);

        return Task.FromResult(verificationCode == cachedCode);
    }
    
    public async Task ConfirmEmailAsync(int verificationCode)
    {
        var user = await userRepository.FindByIdOrThrowAsync(userContext.UserId);

        if (user.Status is not UserStatus.PendingEmailConfirmation)
            throw new ConflictException("Email already confirmed");

        if (!await ValidateVerificationCodeAsync(user.Email, verificationCode))
            throw new ValidationException("Verification code invalid or expired, please try again");

        await userRepository.UpdateUserStatusAsync(user.Id, UserStatus.Normal);
    }
}