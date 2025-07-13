using MongoDB.Bson;

namespace Cineverse.Mongo.Schemas.Base;

public class BaseEntity : IEntity
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
}