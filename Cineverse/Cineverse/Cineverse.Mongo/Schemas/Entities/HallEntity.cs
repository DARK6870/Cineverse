using Cineverse.Mongo.Schemas.Models;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("halls")]
public record HallEntity : TimestampedEntity
{
    public required string Name { get; init; }
    
    public required Seat[] Seats { get; init; }
}