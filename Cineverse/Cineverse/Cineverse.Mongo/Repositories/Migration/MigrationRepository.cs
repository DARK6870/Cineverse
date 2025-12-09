using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Migration;

public class MigrationRepository(
    IMongoDatabase mongoDatabase
): GenericRepository<MigrationEntity>(mongoDatabase), IMigrationRepository
{
    
}