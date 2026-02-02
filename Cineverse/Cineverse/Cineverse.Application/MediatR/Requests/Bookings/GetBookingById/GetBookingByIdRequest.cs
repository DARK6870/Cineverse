using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookingById;

public record GetBookingByIdRequest(string Id) : IRequest<BookingEntity>;