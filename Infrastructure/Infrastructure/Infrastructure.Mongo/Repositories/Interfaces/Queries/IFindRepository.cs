namespace Infrastructure.Mongo.Repositories.Interfaces.Queries;

public interface IFindRepository<T>
{
    Task<T?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<T> FindByIdOrThrowAsync(string id, CancellationToken cancellationToken = default);
}