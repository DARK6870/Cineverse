using System.Linq.Expressions;

namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IDeleteRepository<T>
{
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);

    Task DeleteOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    
    Task DeleteManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}