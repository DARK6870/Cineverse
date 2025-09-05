using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Schemas.Base;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("_migrations")]
public class MigrationEntity(string description) : BaseEntity
{
    public string Description { get; set; } = description;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}