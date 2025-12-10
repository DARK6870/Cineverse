namespace Infrastructure.Mongo.Exceptions;

public class EntityNotFoundException(Type entityType) : Exception
{
    public string ErrorMessage => $"{ entityType.Name.Replace("Entity", "") } was not found";
}