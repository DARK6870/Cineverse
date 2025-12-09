using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookings;

public record GetBookingsRequest : IRequest<IQueryable<BookingEntity>>;