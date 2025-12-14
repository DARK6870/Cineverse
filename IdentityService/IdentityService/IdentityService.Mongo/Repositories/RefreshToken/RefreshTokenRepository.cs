using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace IdentityService.Mongo.Repositories.RefreshToken;

public class RefreshTokenRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<RefreshTokenEntity>(mongoDatabase), IRefreshTokenRepository
{
    
}