using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.UpdateHall;

public record UpdateHallRequest(
    string Id,
    string Name,
    Seat[] Seats
) : IRequest<bool>;