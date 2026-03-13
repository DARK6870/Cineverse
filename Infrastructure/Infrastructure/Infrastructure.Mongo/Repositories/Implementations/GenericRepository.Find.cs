using System.Linq.Expressions;
using Infrastructure.Common.Exceptions;
using Infrastructure.Mongo.Exceptions;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>
{
    public async Task<T?> FindByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var findResult = await Collection
            .FindAsync(x => x.Id == id, cancellationToken: cancellationToken);

        return await findResult.SingleOrDefaultAsync(cancellationToken);
    }
    
    public async Task<T> FindByIdOrThrowAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var findResult = await Collection
            .FindAsync(x => x.Id == id, cancellationToken: cancellationToken);

        return await findResult.SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException(typeof(T));
    }
    
    public async Task<T?> FindFirstAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default
    )
    {
        return await Collection
            .Find(expression)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<T> FindFirstOrThrowAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default
    )
    {
        var result = await Collection
            .Find(expression)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new NotFoundException(typeof(T).Name);
    }
}