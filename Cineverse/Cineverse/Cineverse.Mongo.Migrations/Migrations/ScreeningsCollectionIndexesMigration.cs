using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Migrations.Core;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Migrations.Migrations;
using static MongoDB.Driver.Builders<Cineverse.Mongo.Schemas.Entities.ScreeningEntity>;

public class ScreeningsCollectionIndexesMigration(
    IMongoDatabase mongoDatabase
) : MongoMigration("Screenings: add indexes")
{
    private readonly IMongoCollection<ScreeningEntity> _collection = mongoDatabase.GetCollection<ScreeningEntity>(
        MongoCollectionAttribute.GetCollectionName(typeof(ScreeningEntity))
    );

    public override async Task MigrateAsync()
    {
        var indexModels = new List<CreateIndexModel<ScreeningEntity>>
        {
            new(IndexKeys.Ascending(x => x.Date))
        };

        await _collection.Indexes.CreateManyAsync(indexModels);
    }
}