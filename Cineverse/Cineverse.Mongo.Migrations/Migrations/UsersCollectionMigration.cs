using Cineverse.Mongo.Common.Attribues;
using Cineverse.Mongo.Migrations.Core;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;
using static MongoDB.Driver.Builders<Cineverse.Mongo.Schemas.Entities.UserEntity>;

namespace Cineverse.Mongo.Migrations.Migrations;

public class UsersCollectionMigration(
    IMongoDatabase mongoDatabase
) : MongoMigration("Users: add indexes")
{
    private readonly IMongoCollection<UserEntity> _collection = mongoDatabase.GetCollection<UserEntity>(
        MongoCollectionAttribute.GetCollectionName(typeof(UserEntity))
    );

    public override async Task MigrateAsync()
    {
        var indexModels = new List<CreateIndexModel<UserEntity>>
        {
            new(IndexKeys.Ascending(x => x.Email))
        };

        await _collection.Indexes.CreateManyAsync(indexModels);
    }
}