using Cineverse.Mongo.Repositories.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Screenings.Commands;

public record DeleteScreeningRequest(string Id) : IRequest<bool>;

public class DeleteScreeningRequestHandler(
    IScreeningRepository screeningRepository
) : IRequestHandler<DeleteScreeningRequest, bool>
{
    public async Task<bool> Handle(DeleteScreeningRequest request, CancellationToken cancellationToken)
    {
        await screeningRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}