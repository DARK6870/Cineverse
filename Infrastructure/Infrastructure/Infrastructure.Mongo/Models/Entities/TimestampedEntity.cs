namespace Infrastructure.Mongo.Models.Entities;

public abstract record TimestampedEntity : BaseEntity
{
    public DateTime DateCreated { get; set; }
    
    public DateTime DateModified { get; set; }
}