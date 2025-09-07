using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;

public record CreateBookingRequest(
    string ScreeningId,
    string[] SeatsIds
) : IRequest<bool>;