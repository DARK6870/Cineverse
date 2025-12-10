using Infrastructure.Mongo.Models.Entities;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T> where T : IEntity
{
    public async Task ReplaceOneAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.ReplaceOneAsync(
            x => x.Id == entity.Id, entity,
            cancellationToken: cancellationToken
        );
    }
}