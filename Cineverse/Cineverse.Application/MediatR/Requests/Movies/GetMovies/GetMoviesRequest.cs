using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.GetMovies;

public record GetMoviesRequest : IRequest<IQueryable<MovieEntity>>;