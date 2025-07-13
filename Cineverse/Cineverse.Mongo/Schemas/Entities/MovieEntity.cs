using Cineverse.Mongo.Common.Attribues;
using Cineverse.Mongo.Schemas.Base;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Movies")]
public class MovieEntity : BaseEntity
{
    public required string Title { get; set; }
    
    public required string Description { get; set; }
    
    public required string[] Images { get; set; }
    
    public required DateOnly ReleaseDate { get; set; }
    
    public required int Duration { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}