using System.Linq.Expressions;

namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface ISearchRepository<T>
{
    IQueryable<T> SearchByText(
        string searchText,
        params Expression<Func<T, object?>>[] excludedFields
    );
}