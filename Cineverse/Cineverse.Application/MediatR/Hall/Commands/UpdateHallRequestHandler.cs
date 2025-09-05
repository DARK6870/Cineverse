using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Hall.Commands;

public record UpdateHallRequest(
    string Id,
    string Name,
    Seat[] Seats
) : IRequest<bool>;

public class UpdateHallRequestHandler(
    IHallRepository hallRepository
) : IRequestHandler<UpdateHallRequest, bool>
{
    public async Task<bool> Handle(UpdateHallRequest request, CancellationToken cancellationToken)
    {
        var hall = await hallRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new ApiRequestException("Hall does not exist", HttpStatusCode.BadRequest);

        var updatedHall = new HallEntity
        {
            Id = request.Id,
            Name = request.Name,
            Seats = request.Seats,
            DateCreated = hall.DateCreated
        };
        await hallRepository.ReplaceOneAsync(updatedHall, cancellationToken);
        
        return true;
    }
}