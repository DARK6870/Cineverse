using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.GetMovieById;

public class GetMovieByIdHandler(
    IMovieRepository movieRepository
) : IRequestHandler<GetMovieByIdRequest, MovieEntity>
{
    public async Task<MovieEntity> Handle(GetMovieByIdRequest request, CancellationToken cancellationToken)
    {
        return await movieRepository.FindByIdOrThrowAsync(request.Id, cancellationToken);
    }
}