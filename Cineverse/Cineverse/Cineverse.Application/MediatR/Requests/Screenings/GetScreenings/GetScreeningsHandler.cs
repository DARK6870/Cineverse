using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.GetScreenings;

public class GetScreeningsHandler(
    IScreeningRepository screeningRepository
) : IRequestHandler<GetScreeningsRequest, IQueryable<ScreeningEntity>>
{
    public Task<IQueryable<ScreeningEntity>> Handle(GetScreeningsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(screeningRepository.AsQueryable());
    }
}