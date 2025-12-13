using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace Cineverse.Mongo.Repositories.Movie;

public interface IMovieRepository : IGenericRepository<MovieEntity>
{
    
}