using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>
{
    public IQueryable<T> AsQueryable(AggregateOptions? options = null)
    {
        return Collection.AsQueryable(aggregateOptions: options);
    }
}