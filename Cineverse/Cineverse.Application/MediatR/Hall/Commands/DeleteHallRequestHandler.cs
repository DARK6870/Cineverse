using Cineverse.Mongo.Repositories.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Hall.Commands;

public record DeleteHallRequest(string Id) : IRequest<bool>;

public class DeleteHallRequestHandler(
    IHallRepository hallRepository
) : IRequestHandler<DeleteHallRequest, bool>
{
    public async Task<bool> Handle(DeleteHallRequest request, CancellationToken cancellationToken)
    {
        await hallRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}