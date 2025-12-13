using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Movie;

public class MovieRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<MovieEntity>(mongoDatabase), IMovieRepository
{
    
}