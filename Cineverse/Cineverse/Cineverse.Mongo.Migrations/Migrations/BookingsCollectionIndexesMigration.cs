using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Migrations.Core;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Migrations.Migrations;
using static MongoDB.Driver.Builders<Cineverse.Mongo.Schemas.Entities.BookingEntity>;

public class BookingsCollectionIndexesMigration(
    IMongoDatabase mongoDatabase
) : MongoMigration("Bookings: add indexes")
{
    private readonly IMongoCollection<BookingEntity> _collection = mongoDatabase.GetCollection<BookingEntity>(
        MongoCollectionAttribute.GetCollectionName(typeof(BookingEntity))
    );

    public override async Task MigrateAsync()
    {
        var indexModels = new List<CreateIndexModel<BookingEntity>>
        {
            new(IndexKeys.Ascending(x => x.ScreeningId)),
        };

        await _collection.Indexes.CreateManyAsync(indexModels);
    }
}