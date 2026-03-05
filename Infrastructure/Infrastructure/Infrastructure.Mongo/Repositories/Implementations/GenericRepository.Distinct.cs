using System.Linq.Expressions;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>
{
    public async Task<IEnumerable<TField>> GetDistinctFieldValuesAsync<TField>(
        Expression<Func<T, TField>> fieldSelector,
        CancellationToken cancellationToken
    )
    {
        var cursor = await Collection.DistinctAsync(
            fieldSelector,
            FilterDefinition<T>.Empty,
            cancellationToken: cancellationToken
        );
        
        return await cursor.ToListAsync(cancellationToken);
    }
}