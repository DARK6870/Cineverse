using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Queries;

public record GetMovieByIdRequest(string Id) : IRequest<MovieEntity?>;

public class GetMovieByIdRequestHandler(
    IMovieRepository movieRepository
) : IRequestHandler<GetMovieByIdRequest, MovieEntity?>
{
    public async Task<MovieEntity?> Handle(GetMovieByIdRequest request, CancellationToken cancellationToken)
    {
        return await movieRepository.FindByIdAsync(request.Id, cancellationToken);
    }
}