using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Hall.Queries;

public record GetHallByIdRequest(string Id) : IRequest<HallEntity?>;

public class GetHallByIdRequestHandler(
    IHallRepository hallRepository
) : IRequestHandler<GetHallByIdRequest, HallEntity?>
{
    public async Task<HallEntity?> Handle(GetHallByIdRequest request, CancellationToken cancellationToken)
    {
        return await hallRepository.FindByIdAsync(request.Id, cancellationToken);
    }
}