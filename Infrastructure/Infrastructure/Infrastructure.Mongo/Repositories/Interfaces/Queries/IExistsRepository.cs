using System.Linq.Expressions;

namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IExistsRepository<T>
{
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task ExistOrThrowAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}