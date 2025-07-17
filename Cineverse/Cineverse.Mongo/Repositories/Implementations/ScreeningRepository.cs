using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Implementations;

public class ScreeningRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<ScreeningEntity>(mongoDatabase), IScreeningRepository
{
    
}