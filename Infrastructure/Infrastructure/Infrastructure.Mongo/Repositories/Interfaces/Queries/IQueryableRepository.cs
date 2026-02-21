using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IQueryableRepository<T>
{
    IQueryable<T> AsQueryable(AggregateOptions? options = null);
}