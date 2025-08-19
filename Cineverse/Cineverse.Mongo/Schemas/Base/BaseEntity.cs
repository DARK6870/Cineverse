using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Base;

public class BaseEntity : IEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
}