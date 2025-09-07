using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Common.Constants;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Notification;
using Cineverse.Notifications.Services.Notification.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cineverse.Identity.Services.EmailVerification;

internal class VerificationService(
    IMemoryCache cache,
    INotificationService notificationService,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IVerificationService
{
    public async Task<int> GenerateVerificationCodeAsync(string email, string fullName)
    {
        if (cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var _))
            throw new ApiRequestException("You already received verification code, try again later", HttpStatusCode.BadRequest);
        
        // TODO: another method
        var random = new Random();
        var verificationCode = random.Next(11111, 99999);
        
        cache.Set(
            CacheConstants.VerificationCodeCacheKey(email),
            verificationCode,
            TimeSpan.FromMinutes(CacheConstants.VerificationCodeCacheLifetimeMinutes)
        );

        await SendVerificationEmailAsync(email, fullName, verificationCode);
        return verificationCode;
    }

    public Task<bool> ValidateVerificationCodeAsync(string email, int verificationCode)
    {
        if (!cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var cachedCode))
            return Task.FromResult(false);

        return Task.FromResult(verificationCode == cachedCode);
    }

    private async Task SendVerificationEmailAsync(string email, string fullName, int verificationCode)
    {
        var confirmEmailUrl = notificationLinksOptions.Value.BaseUrl + notificationLinksOptions.Value.ConfirmEmailPath;

        await notificationService.SendVerificationEmailAsync(
            email,
            fullName,
            verificationCode,
            confirmEmailUrl
        );
    }
}