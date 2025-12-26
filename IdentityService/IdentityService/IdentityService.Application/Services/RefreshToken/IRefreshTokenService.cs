using IdentityService.Mongo.Schemas.Entities;

namespace IdentityService.Application.Services.RefreshToken;

public interface IRefreshTokenService
{
    /// <summary>
    /// Create or update an existing token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ipAddress"></param>
    /// <returns>Raw refresh token</returns>
    Task<string> CreateOrUpdateTokenAsync(string userId, string ipAddress);
}