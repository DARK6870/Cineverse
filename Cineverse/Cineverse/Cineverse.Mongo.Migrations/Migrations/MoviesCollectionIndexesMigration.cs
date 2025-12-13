using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Migrations.Core;
using MongoDB.Driver;
using static MongoDB.Driver.Builders<Cineverse.Mongo.Schemas.Entities.MovieEntity>;

namespace Cineverse.Mongo.Migrations.Migrations;

public class MoviesCollectionIndexesMigration(
    IMongoDatabase mongoDatabase
) : MongoMigration("Movies: add indexes")
{
    private readonly IMongoCollection<MovieEntity> _collection = mongoDatabase.GetCollection<MovieEntity>(
        MongoCollectionAttribute.GetCollectionName(typeof(MovieEntity))
    );

    public override async Task MigrateAsync()
    {
        var indexModels = new List<CreateIndexModel<MovieEntity>>
        {
            new(IndexKeys.Ascending(x => x.Title)),
            new(IndexKeys.Ascending(x => x.IsAvailable))
        };

        await _collection.Indexes.CreateManyAsync(indexModels);
    }
}