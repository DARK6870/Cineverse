using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.CreateHall;

public record CreateHallRequest(
    string Name,
    List<Seat> Seats
) : IRequest<bool>;