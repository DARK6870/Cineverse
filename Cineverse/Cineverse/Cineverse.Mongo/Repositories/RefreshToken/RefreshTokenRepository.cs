using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.RefreshToken;

public class RefreshTokenRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<RefreshTokenEntity>(mongoDatabase), IRefreshTokenRepository
{
    
}