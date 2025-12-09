using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Hall;

public class HallRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<HallEntity>(mongoDatabase), IHallRepository
{
    
}