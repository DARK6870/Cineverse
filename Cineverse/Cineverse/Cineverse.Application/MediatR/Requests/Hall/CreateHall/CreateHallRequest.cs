using Cineverse.Application.Common.Models.Requests;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.CreateHall;

public record CreateHallRequest(
    string Name,
    List<SeatRequest> Seats
) : IRequest<string>;