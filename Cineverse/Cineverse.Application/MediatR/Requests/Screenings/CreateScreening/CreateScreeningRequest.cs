using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.CreateScreening;

public record CreateScreeningRequest(
    string MovieId,
    string HallId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int TicketPrice
) : IRequest<bool>;
