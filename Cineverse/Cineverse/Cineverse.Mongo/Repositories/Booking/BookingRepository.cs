using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Booking;

public class BookingRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<BookingEntity>(mongoDatabase), IBookingRepository
{
    
}