using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetUserBookings;

public record GetUserBookingsRequest() : IRequest<IQueryable<BookingEntity>>;