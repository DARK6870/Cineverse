using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Movie;

public class MovieRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<MovieEntity>(mongoDatabase), IMovieRepository
{
    
}