using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Common.Exceptions;
using Infrastructure.Common.Helpers;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace IdentityService.Mongo.Repositories.RefreshToken;

public class RefreshTokenRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<RefreshTokenEntity>(mongoDatabase), IRefreshTokenRepository
{
    public async Task<RefreshTokenEntity?> GetActiveTokenAsync(string userId, string ipAddress)
    {
        return await FindFirstAsync(x =>
            x.UserId == userId &&
            x.IpAddress == ipAddress
        );
    }

    public async Task<RefreshTokenEntity> GetRefreshTokenOrThrowAsync(string refreshToken, string ipAddress)
    {
        var hashedToken = HashHelper.ComputeSha256(refreshToken);
        
        return await FindFirstAsync(x => 
                x.TokenHash == hashedToken && 
                x.IpAddress == ipAddress
            ) ?? throw new UnauthorizedException("No active sessions found");
    }
}