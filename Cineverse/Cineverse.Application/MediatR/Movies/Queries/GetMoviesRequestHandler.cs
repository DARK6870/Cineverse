using System.Diagnostics.CodeAnalysis;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Queries;

public record GetMoviesRequest : IRequest<IQueryable<MovieEntity>>;

public class GetMoviesRequestHandler(
    IMovieRepository movieRepository
) : IRequestHandler<GetMoviesRequest, IQueryable<MovieEntity>>
{
    public Task<IQueryable<MovieEntity>> Handle(GetMoviesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(movieRepository.AsQueryable());
    }
}