using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookedSeats;

public record GetBookedSeatsRequest(string ScreeningId) : IRequest<List<string>>;