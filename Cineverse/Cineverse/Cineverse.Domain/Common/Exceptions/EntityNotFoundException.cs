namespace Cineverse.Domain.Common.Exceptions;

public class EntityNotFoundException(Type entityType) : Exception
{
    public string ErrorMessage => $"{ entityType.Name.Replace("Entity", "") } was not found";
}