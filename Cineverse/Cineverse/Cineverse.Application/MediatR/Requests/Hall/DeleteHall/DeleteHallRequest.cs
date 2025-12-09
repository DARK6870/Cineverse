using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.DeleteHall;

public record DeleteHallRequest(string Id) : IRequest<bool>;