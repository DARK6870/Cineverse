using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;

namespace Infrastructure.Mongo.Migrations.Entities;

[MongoCollection("_migrations")]
public record MigrationEntity(string Description) : BaseEntity
{
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}