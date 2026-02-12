namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IReplaceRepository<T>
{
    Task ReplaceOneAsync(T entity, CancellationToken cancellationToken = default);
}