using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Schemas.Base;
using Cineverse.Mongo.Schemas.Models;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Halls")]
public class HallEntity : BaseEntity
{
    public required string Name { get; set; }
    
    public required Seat[] Seats { get; set; }
    
    public DateTime DateCreated { get; set; } =  DateTime.UtcNow;
}