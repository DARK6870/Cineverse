using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Hall.Queries;

public record GetHallsRequest : IRequest<IQueryable<HallEntity>>;

public class GetHallsRequestHandler(
    IHallRepository hallRepository
) : IRequestHandler<GetHallsRequest, IQueryable<HallEntity>>
{
    public Task<IQueryable<HallEntity>> Handle(GetHallsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(hallRepository.AsQueryable());
    }
}