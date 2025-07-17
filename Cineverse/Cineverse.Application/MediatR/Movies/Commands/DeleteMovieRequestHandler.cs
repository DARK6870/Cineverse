using Cineverse.Mongo.Repositories.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Commands;

public record DeleteMovieRequest(string Id) : IRequest<bool>;

public class DeleteMovieRequestHandler(
    IMovieRepository movieRepository
) : IRequestHandler<DeleteMovieRequest, bool>
{
    public async Task<bool> Handle(DeleteMovieRequest request, CancellationToken cancellationToken)
    {
        await movieRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}