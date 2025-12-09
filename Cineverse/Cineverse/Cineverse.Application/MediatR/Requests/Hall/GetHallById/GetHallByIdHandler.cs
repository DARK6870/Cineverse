using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.GetHallById;

public class GetHallByIdHandler(
    IHallRepository hallRepository
) : IRequestHandler<GetHallByIdRequest, HallEntity?>
{
    public async Task<HallEntity?> Handle(GetHallByIdRequest request, CancellationToken cancellationToken)
    {
        return await hallRepository.FindByIdAsync(request.Id, cancellationToken);
    }
}