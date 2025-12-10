using Infrastructure.Mongo.Models.Entities;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T> where T : IEntity
{
    public async Task InsertOneAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task InsertManyAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        var documents = entities.ToList();
        if (documents.Count == 0)
            return;

        await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
    }
}