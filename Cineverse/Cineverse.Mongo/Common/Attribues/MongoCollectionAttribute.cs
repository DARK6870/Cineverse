namespace Cineverse.Mongo.Common.Attribues;

[AttributeUsage(AttributeTargets.Class)]
public class MongoCollectionAttribute(string collectionName) : Attribute
{
    private string CollectionName { get; } = collectionName;

    public static string GetCollectionName(Type entityType)
    {
        var attribute = entityType.GetCustomAttributes(typeof(MongoCollectionAttribute), false)
            .FirstOrDefault() as MongoCollectionAttribute;

        return attribute?.CollectionName ?? throw new NullReferenceException("Collection name can not be null");
    }
}