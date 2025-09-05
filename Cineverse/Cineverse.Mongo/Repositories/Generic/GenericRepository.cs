using System.Linq.Expressions;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Schemas.Base;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Generic;

public class GenericRepository<T>(
    IMongoDatabase mongoDatabase
) : IGenericRepository<T> where T : IEntity
{
    protected IMongoCollection<T> Collection => mongoDatabase.GetCollection<T>(MongoCollectionAttribute.GetCollectionName(typeof(T)));

    public IQueryable<T> AsQueryable(AggregateOptions? options = null)
    {
        return Collection.AsQueryable(aggregateOptions: options);
    }

    public async Task<T?> FindByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var findResult = await Collection
            .FindAsync(e => e.Id == id, cancellationToken: cancellationToken);

        return await findResult.SingleOrDefaultAsync(cancellationToken);
    }
    
    public async Task<T> FindByIdAndThrowAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var findResult = await Collection
            .FindAsync(e => e.Id == id, cancellationToken: cancellationToken);

        return await findResult.SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException(typeof(T));
    }

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

    public async Task DeleteByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.DeleteOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
    }
    
    public async Task DeleteOneAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task ReplaceOneAsync(
        T entity,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default
    )
    {
        return await Collection.CountDocumentsAsync(filter, new CountOptions(), cancellationToken) > 0L;
    }
    
    public async Task ExistOrThrowAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default
    )
    {
        if (await Collection.CountDocumentsAsync(filter, new CountOptions(), cancellationToken) == 0L)
            throw new EntityNotFoundException(typeof(T));
    }
}