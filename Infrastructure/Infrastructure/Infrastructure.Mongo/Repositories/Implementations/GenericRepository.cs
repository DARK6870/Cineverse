using System.Linq.Expressions;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Exceptions;
using Infrastructure.Mongo.Models.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>(
    IMongoDatabase mongoDatabase
) : IGenericRepository<T> where T : IEntity
{
    protected IMongoCollection<T> Collection => mongoDatabase.GetCollection<T>(MongoCollectionAttribute.GetCollectionName(typeof(T)));

    public IQueryable<T> AsQueryable(AggregateOptions? options = null)
    {
        return Collection.AsQueryable(aggregateOptions: options);
    }
}