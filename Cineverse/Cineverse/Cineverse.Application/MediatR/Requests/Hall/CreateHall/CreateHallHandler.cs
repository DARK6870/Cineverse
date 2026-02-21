using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.CreateHall;

public class CreateHallHandler(
    IHallRepository hallRepository
) : IRequestHandler<CreateHallRequest, bool>
{
    public async Task<bool> Handle(CreateHallRequest request, CancellationToken cancellationToken)
    {
        var hall = new HallEntity
        {
            Name = request.Name,
            Seats = request.Seats.ToArray()
        };
        await hallRepository.InsertOneAsync(hall, cancellationToken);
        
        return true;
    }
}