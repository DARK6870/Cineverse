using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.RefreshToken;

public class RefreshTokenRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<RefreshTokenEntity>(mongoDatabase), IRefreshTokenRepository
{
    
}