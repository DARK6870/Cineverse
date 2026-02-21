namespace Infrastructure.Mongo.Repositories.Interfaces.Commands;

public interface IReplaceRepository<T>
{
    Task ReplaceOneAsync(T entity, CancellationToken cancellationToken = default);
}