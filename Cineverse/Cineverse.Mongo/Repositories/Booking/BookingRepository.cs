using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Booking;

public class BookingRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<BookingEntity>(mongoDatabase), IBookingRepository
{
    
}