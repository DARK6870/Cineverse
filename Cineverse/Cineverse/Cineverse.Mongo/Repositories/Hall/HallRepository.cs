using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Hall;

public class HallRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<HallEntity>(mongoDatabase), IHallRepository
{
    
}