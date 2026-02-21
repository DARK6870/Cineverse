using Infrastructure.Mongo.Models.Entities;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>
{
    public async Task ReplaceOneAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        if (entity is TimestampedEntity timestamped)
            timestamped.DateModified = DateTime.UtcNow;
        
        await Collection.ReplaceOneAsync(
            x => x.Id == entity.Id, entity,
            cancellationToken: cancellationToken
        );
    }
}