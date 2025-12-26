using System.Net;
using IdentityService.Application.Constants;
using IdentityService.Application.Helpers;
using Infrastructure.WebApi.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Application.Services.EmailVerification;

public class EmailVerificationService(
    IMemoryCache cache
    //INotificationService notificationService,
) : IEmailVerificationService
{
    public async Task<int> GenerateAndSendVerificationCodeAsync(string email, string fullName)
    {
        if (cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var _))
            throw new ApiRequestException("You already received verification code, try again later", HttpStatusCode.BadRequest);

        var verificationCode = EmailVerificationHelper.GenerateVerificationCode();
        
        cache.Set(
            CacheConstants.VerificationCodeCacheKey(email),
            verificationCode,
            TimeSpan.FromMinutes(CacheConstants.VerificationCodeCacheLifetimeMinutes)
        );

        // TODO: send notification
        /*var actionUrl = notificationLinksOptions.Value.BuildBookingDetailsUrl(email);
        await notificationService.SendVerificationEmailAsync(
            email,
            fullName,
            verificationCode,
            actionUrl
        );*/
        await Task.Delay(1);
        return verificationCode;
    }

    public Task<bool> ValidateVerificationCodeAsync(string email, int verificationCode)
    {
        if (!cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var cachedCode))
            return Task.FromResult(false);

        return Task.FromResult(verificationCode == cachedCode);
    }
}