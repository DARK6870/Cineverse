using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("_migrations")]
public class MigrationEntity(string description) : BaseEntity
{
    public string Description { get; set; } = description;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}