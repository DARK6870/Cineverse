using Cineverse.Mongo.Repositories.Screening;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.DeleteScreening;

public class DeleteScreeningHandler(
    IScreeningRepository screeningRepository
) : IRequestHandler<DeleteScreeningRequest, bool>
{
    public async Task<bool> Handle(DeleteScreeningRequest request, CancellationToken cancellationToken)
    {
        await screeningRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}