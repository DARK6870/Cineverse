using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace IdentityService.Mongo.Repositories.RefreshToken;

public interface IRefreshTokenRepository : IGenericRepository<RefreshTokenEntity>
{
    
}