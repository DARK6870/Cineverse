using System.Net;
using Infrastructure.Common.Exceptions.Base;

namespace Infrastructure.Mongo.Exceptions;

public class EntityNotFoundException(Type entityType) : BaseException($"{entityType.Name.Replace("Entity", "")} was not found", HttpStatusCode.NotFound)
{
    public Type EntityType { get; } = entityType;
}