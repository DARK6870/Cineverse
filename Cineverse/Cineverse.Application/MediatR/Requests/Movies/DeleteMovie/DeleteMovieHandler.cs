using Cineverse.Mongo.Repositories.Movie;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.DeleteMovie;

public class DeleteMovieHandler(
    IMovieRepository movieRepository
) : IRequestHandler<DeleteMovieRequest, bool>
{
    public async Task<bool> Handle(DeleteMovieRequest request, CancellationToken cancellationToken)
    {
        await movieRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}