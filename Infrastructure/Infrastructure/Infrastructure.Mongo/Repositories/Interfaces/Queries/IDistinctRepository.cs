using System.Linq.Expressions;

namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IDistinctRepository<T>
{
    Task<IEnumerable<TField>> GetDistinctFilterValuesAsync<TField>(
        Expression<Func<T, TField>> fieldSelector,
        CancellationToken cancellationToken
    );
}