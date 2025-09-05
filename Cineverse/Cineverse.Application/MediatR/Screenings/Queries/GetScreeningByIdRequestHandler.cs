using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Screenings.Queries;

public record GetScreeningByIdRequest(string Id) : IRequest<ScreeningEntity?>;

public class GetScreeningByIdRequestHandler(
    IScreeningRepository screeningRepository
) : IRequestHandler<GetScreeningByIdRequest, ScreeningEntity?>
{
    public async Task<ScreeningEntity?> Handle(GetScreeningByIdRequest request, CancellationToken cancellationToken)
    {
        return await screeningRepository.FindByIdAsync(request.Id, cancellationToken);
    }
}