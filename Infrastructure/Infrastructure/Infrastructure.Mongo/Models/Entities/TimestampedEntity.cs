namespace Infrastructure.Mongo.Models.Entities;

public abstract record TimestampedEntity : BaseEntity
{
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}