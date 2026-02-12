using Cineverse.Mongo.Schemas.Models;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Halls")]
public class HallEntity : BaseEntity
{
    public required string Name { get; set; }
    
    public required Seat[] Seats { get; set; }
    
    public DateTime DateCreated { get; set; } =  DateTime.UtcNow;
}