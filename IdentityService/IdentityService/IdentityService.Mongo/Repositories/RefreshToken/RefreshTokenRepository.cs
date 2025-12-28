using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Common.Helpers;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IdentityService.Mongo.Repositories.RefreshToken;

public class RefreshTokenRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<RefreshTokenEntity>(mongoDatabase), IRefreshTokenRepository
{
    public async Task<RefreshTokenEntity?> GetActiveTokenAsync(string userId, string ipAddress)
    {
        return await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => 
                x.UserId == userId && 
                x.IpAddress == ipAddress
            );
    }

    public async Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken, string ipAddress)
    {
        var hashedToken = HashHelper.ComputeSha256(refreshToken);
        
        return await AsQueryable()
            .FirstOrDefaultAsync(x => 
                x.TokenHash == hashedToken && 
                x.IpAddress == ipAddress
            );
    }
}