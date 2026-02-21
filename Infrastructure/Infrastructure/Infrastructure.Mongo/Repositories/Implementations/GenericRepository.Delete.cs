using System.Linq.Expressions;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>
{
    public async Task DeleteByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.DeleteOneAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }
    
    public async Task DeleteOneAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default
    )
    {
        await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task DeleteManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        await Collection.DeleteManyAsync(filter, cancellationToken);
    }
}