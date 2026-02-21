using Infrastructure.Mongo.Models.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Commands;
using Infrastructure.Mongo.Repositories.Interfaces.Queries;

namespace Infrastructure.Mongo.Repositories.Interfaces.Generic;

public interface IGenericRepository<T> :
    IQueryableRepository<T>,
    IFindRepository<T>,
    IExistsRepository<T>,
    IInsertRepository<T>,
    IReplaceRepository<T>,
    IDeleteRepository<T>,
    IDistinctRepository<T>,
    ISearchRepository<T>
where T : BaseEntity;