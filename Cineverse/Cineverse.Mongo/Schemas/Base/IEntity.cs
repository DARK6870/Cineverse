using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Base;

public interface IEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}