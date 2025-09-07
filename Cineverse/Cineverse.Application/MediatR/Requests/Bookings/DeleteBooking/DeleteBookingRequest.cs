using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.DeleteBooking;

public record DeleteBookingRequest(string Id) : IRequest<bool>;