using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Screening;

public class ScreeningRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<ScreeningEntity>(mongoDatabase), IScreeningRepository
{
    
}