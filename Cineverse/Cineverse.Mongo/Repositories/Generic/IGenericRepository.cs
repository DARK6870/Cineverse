using System.Linq.Expressions;
using Cineverse.Mongo.Schemas.Base;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Generic;

public interface IGenericRepository<T> where T : IEntity
{
    IQueryable<T> AsQueryable(AggregateOptions? options = null);
    
    Task<T?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<T> FindByIdOrThrowAsync(string id, CancellationToken cancellationToken = default);
    
    Task InsertOneAsync(T entity, CancellationToken cancellationToken = default);
    
    Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);

    Task DeleteOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    
    Task ReplaceOneAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task ExistOrThrowAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}