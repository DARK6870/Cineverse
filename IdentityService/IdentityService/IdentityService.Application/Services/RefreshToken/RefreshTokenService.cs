using IdentityService.Application.Common.Helpers;
using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Common.Helpers;

namespace IdentityService.Application.Services.RefreshToken;

public class RefreshTokenService(
    IRefreshTokenRepository refreshTokenRepository
) : IRefreshTokenService
{
    public async Task<string> CreateOrUpdateTokenAsync(string userId, string ipAddress)
    {
        // get existing refresh token
        var existingToken = await refreshTokenRepository.GetActiveTokenAsync(userId, ipAddress);

        if (existingToken is not null)
            await refreshTokenRepository.DeleteByIdAsync(existingToken.Id);
        
        // generate raw token
        var rawToken = RefreshTokenHelper.Generate();
        
        // hash war token
        var hashedToken = HashHelper.ComputeSha256(rawToken);
        
        // create new token entity
        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = userId,
            IpAddress = ipAddress,
            TokenHash = hashedToken
        };
        await refreshTokenRepository.InsertOneAsync(refreshTokenEntity);
        
        return rawToken;
    }
}