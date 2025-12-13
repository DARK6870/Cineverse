using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Migrations.Core;
using MongoDB.Driver;
using static MongoDB.Driver.Builders<Cineverse.Mongo.Schemas.Entities.RefreshTokenEntity>;

namespace Cineverse.Mongo.Migrations.Migrations;

public class RefreshTokensCollectionIndexesMigration(
    IMongoDatabase mongoDatabase
) : MongoMigration("RefreshTokens: add TTL indexes")
{
    private readonly IMongoCollection<RefreshTokenEntity> _collection = mongoDatabase.GetCollection<RefreshTokenEntity>(
        MongoCollectionAttribute.GetCollectionName(typeof(RefreshTokenEntity))
    );

    public override async Task MigrateAsync()
    {
        var indexOptions = new CreateIndexOptions
        {
            ExpireAfter = TimeSpan.FromDays(30)
        };
        var indexKeys = IndexKeys.Ascending(x => x.DateCreated);
        
        await _collection.Indexes.CreateOneAsync(new CreateIndexModel<RefreshTokenEntity>(indexKeys, indexOptions));
    }
}