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
}