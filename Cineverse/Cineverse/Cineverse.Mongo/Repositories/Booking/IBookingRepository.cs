using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace Cineverse.Mongo.Repositories.Booking;

public interface IBookingRepository : IGenericRepository<BookingEntity>
{
    
}