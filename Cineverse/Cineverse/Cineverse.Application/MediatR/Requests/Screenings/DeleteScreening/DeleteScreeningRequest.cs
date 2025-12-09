using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.DeleteScreening;

public record DeleteScreeningRequest(string Id) : IRequest<bool>;