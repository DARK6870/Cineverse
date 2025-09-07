using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.GetHalls;

public class GetHallsHandler(
    IHallRepository hallRepository
) : IRequestHandler<GetHallsRequest, IQueryable<HallEntity>>
{
    public Task<IQueryable<HallEntity>> Handle(GetHallsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(hallRepository.AsQueryable());
    }
}