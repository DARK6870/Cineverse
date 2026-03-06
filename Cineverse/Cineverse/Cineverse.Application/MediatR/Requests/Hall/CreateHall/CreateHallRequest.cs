using Cineverse.Mongo.Schemas.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.CreateHall;

public record SeatRequest(int Row, int Number);

public record CreateHallRequest(
    string Name,
    List<SeatRequest> Seats
) : IRequest<bool>;