using System.Linq.Expressions;

namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IFindRepository<T>
{
    Task<T?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<T> FindByIdOrThrowAsync(string id, CancellationToken cancellationToken = default);

    Task<T?> FindFirstAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    
    Task<T> FindFirstOrThrowAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
}