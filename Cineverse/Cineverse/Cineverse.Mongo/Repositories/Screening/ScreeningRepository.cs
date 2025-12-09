using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Screening;

public class ScreeningRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<ScreeningEntity>(mongoDatabase), IScreeningRepository
{
    
}