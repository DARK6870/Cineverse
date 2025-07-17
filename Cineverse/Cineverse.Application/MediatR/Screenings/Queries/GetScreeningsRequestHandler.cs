using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Screenings.Queries;

public record GetScreeningsRequest : IRequest<IQueryable<ScreeningEntity>>;

public class GetScreeningsRequestHandler(
    IScreeningRepository screeningRepository
) : IRequestHandler<GetScreeningsRequest, IQueryable<ScreeningEntity>>
{
    public Task<IQueryable<ScreeningEntity>> Handle(GetScreeningsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(screeningRepository.AsQueryable());
    }
}