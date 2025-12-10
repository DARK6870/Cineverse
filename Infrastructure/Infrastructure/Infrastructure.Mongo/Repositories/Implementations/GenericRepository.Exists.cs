using System.Linq.Expressions;
using Infrastructure.Mongo.Exceptions;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T> where T : IEntity
{
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