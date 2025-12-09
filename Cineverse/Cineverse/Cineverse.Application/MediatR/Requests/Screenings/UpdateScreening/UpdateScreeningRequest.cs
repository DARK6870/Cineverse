using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.UpdateScreening;

public record UpdateScreeningRequest(
    string Id,
    string MovieId,
    string HallId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int TicketPrice
) : IRequest<bool>;