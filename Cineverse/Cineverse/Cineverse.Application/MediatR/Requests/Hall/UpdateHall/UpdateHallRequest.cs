using Cineverse.Application.Common.Models.Requests;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.UpdateHall;

public record UpdateHallRequest(
    string Id,
    string Name,
    SeatRequest[] Seats
) : IRequest<bool>;