using System.Net;
using IdentityService.Application.Constants;
using IdentityService.Application.Notifications.NotificationClientExtensions;
using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.WebApi.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;
using NotificationService.Client.Models.Options;
using NotificationService.Client.Services;

namespace IdentityService.Application.Services.Password;

public class RestorePasswordService(
    IMemoryCache  cache,
    IUserRepository userRepository,
    INotificationServiceClient notificationServiceClient,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IRestorePasswordService
{
    public async Task GenerateAndSendPasswordResetCode(string email)
    {
        var user = await userRepository.AsQueryable().FirstOrDefaultAsync(x => x.Email == email)
            ?? throw new ApiRequestException("Email does not exist", HttpStatusCode.BadRequest);
        
        if (cache.TryGetValue<string>(CacheConstants.RestorePasswordCacheKey(email), out _))
            throw new ApiRequestException("You already received restore password link, try again later", HttpStatusCode.BadRequest);
        
        var code = Guid.NewGuid().ToString();
        cache.Set(
            CacheConstants.RestorePasswordCacheKey(email),
            code,
            TimeSpan.FromMinutes(CacheConstants.RestorePasswordCacheLifetimeMinutes)
        );
        
        var actionUrl = notificationLinksOptions.Value.BuildRestorePasswordUrl(email, code);
        await notificationServiceClient.SendRestorePasswordEmailAsync(
            email,
            user.GetFullName(),
            actionUrl
        );
    }

    public async Task<bool> RestorePasswordByCode(string email, string code, string password)
    {
        var user = await userRepository.AsQueryable().FirstOrDefaultAsync(x => x.Email == email)
                   ?? throw new ApiRequestException("Email does not exist", HttpStatusCode.BadRequest);

        if (!cache.TryGetValue<string>(CacheConstants.RestorePasswordCacheKey(email), out var cachedCode) || code != cachedCode)
            throw new ApiRequestException("The restore password link is invalid or has expired, please try again.", HttpStatusCode.BadRequest);
        
        if (await userRepository.GetUserByCredentialsAsync(email, password) is not null)
            throw new ApiRequestException("New password must be different from the current password", HttpStatusCode.BadRequest);


        var result = await userRepository.UpdateUserPasswordAsync(user.Id, password);
        await refreshTokenRepository.DeleteManyAsync(x => x.UserId == user.Id);
        cache.Remove(CacheConstants.RestorePasswordCacheKey(email));
        
        return result;
    }
}