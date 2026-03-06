using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.UpdateHall;

public class UpdateHallHandler(
    IHallRepository hallRepository
) : IRequestHandler<UpdateHallRequest, bool>
{
    public async Task<bool> Handle(UpdateHallRequest request, CancellationToken cancellationToken)
    {
        var seats = request.Seats
            .Select(s => new Seat
            {
                SeatId = $"{s.Row}-{s.Number}",
                Row = s.Row,
                Number = s.Number
            })
            .ToArray();
        
        var hall = await hallRepository.FindByIdOrThrowAsync(request.Id, cancellationToken);
        
        var updatedHall = new HallEntity
        {
            Id = request.Id,
            Name = request.Name,
            Seats = seats,
            DateCreated = hall.DateCreated
        };
        await hallRepository.ReplaceOneAsync(updatedHall, cancellationToken);
        
        return true;
    }
}