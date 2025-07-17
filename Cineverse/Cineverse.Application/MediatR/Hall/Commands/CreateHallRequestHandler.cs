using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Hall.Commands;

public record CreateHallRequest(
    string Name,
    Seat[] Seats
) : IRequest<bool>;

public class CreateHallRequestHandler(
    IHallRepository hallRepository
) : IRequestHandler<CreateHallRequest, bool>
{
    public async Task<bool> Handle(CreateHallRequest request, CancellationToken cancellationToken)
    {
        var hall = new HallEntity
        {
            Name = request.Name,
            Seats = request.Seats
        };
        
        await hallRepository.InsertOneAsync(hall, cancellationToken);
        return true;
    }
}