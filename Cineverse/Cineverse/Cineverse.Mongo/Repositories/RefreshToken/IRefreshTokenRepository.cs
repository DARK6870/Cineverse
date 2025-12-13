using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace Cineverse.Mongo.Repositories.RefreshToken;

public interface IRefreshTokenRepository : IGenericRepository<RefreshTokenEntity>
{
    
}