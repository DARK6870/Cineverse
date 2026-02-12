namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IInsertRepository<T>
{
    Task InsertOneAsync(T entity, CancellationToken cancellationToken = default);
    
    Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}