using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace IdentityService.Mongo.Repositories.RefreshToken;

public interface IRefreshTokenRepository : IGenericRepository<RefreshTokenEntity>
{
    Task<RefreshTokenEntity?> GetActiveTokenAsync(string userId, string ipAddress);
    
    Task<RefreshTokenEntity> GetRefreshTokenOrThrowAsync(string refreshToken, string ipAddress);
}