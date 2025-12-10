using Infrastructure.Mongo.Models.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Queries;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Interfaces.Generic;

public interface IGenericRepository<T> :
    IFindRepository<T>,
    IExistsRepository<T>,
    IInsertRepository<T>,
    IReplaceRepository<T>,
    IDeleteRepository<T>
    where T : IEntity
{
    IQueryable<T> AsQueryable(AggregateOptions? options = null);
}