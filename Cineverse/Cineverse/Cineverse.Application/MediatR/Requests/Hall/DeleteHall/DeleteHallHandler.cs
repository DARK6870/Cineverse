using Cineverse.Mongo.Repositories.Hall;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.DeleteHall;

public class DeleteHallHandler(
    IHallRepository hallRepository
) : IRequestHandler<DeleteHallRequest, bool>
{
    public async Task<bool> Handle(DeleteHallRequest request, CancellationToken cancellationToken)
    {
        await hallRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}