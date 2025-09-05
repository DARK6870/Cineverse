using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Common.Constants;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Cineverse.Identity.Services.EmailVerification;

internal class VerificationService(
    IMemoryCache cache,
    INotificationService notificationService
) : IVerificationService
{
    public async Task<int> GenerateVerificationCodeAsync(string email, string fullName)
    {
        if (cache.TryGetValue<int>(CacheConstants.VerificationCodeCacheKey(email), out var _))
            throw new ApiRequestException("You already received verification code, try again later", HttpStatusCode.BadRequest);
        
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
        var notification = new MessageBuilder
        {
            Title = "Email Confirmation Code",
            FullName = fullName,
            Message = "Please complete your account setup to explore our website without restrictions<br><br>" +
                      $"Your verification code is <b>{verificationCode}</b><br>" +
                      $"<small>The code will be valid for {CacheConstants.VerificationCodeCacheLifetimeMinutes} minutes</small>",
            ActionUrl = "https://localhost/confirm/" + verificationCode,
            ActionText = "to confirm your email"
        };
        
        await notificationService.SendEmailNotification(email, notification.Title, notification);
    }
}