using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.CreateHall;

public class CreateHallHandler(
    IHallRepository hallRepository
) : IRequestHandler<CreateHallRequest, string>
{
    public async Task<string> Handle(CreateHallRequest request, CancellationToken cancellationToken)
    {
        var seats = request.Seats
            .Select(s => new Seat
            {
                SeatId = $"{s.Row}-{s.Number}",
                Row = s.Row,
                Number = s.Number
            })
            .ToArray();
        
        var hall = new HallEntity
        {
            Name = request.Name,
            Seats = seats
        };
        await hallRepository.InsertOneAsync(hall, cancellationToken);
        
        return hall.Id;
    }
}