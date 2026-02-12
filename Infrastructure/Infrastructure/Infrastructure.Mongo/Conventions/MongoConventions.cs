using MongoDB.Bson.Serialization.Conventions;

namespace Infrastructure.Mongo.Conventions;

public class MongoConventions
{
    private const string ConventionPackName = "DefaultMongoConventions";

    private static bool _registered;

    public static void Register()
    {
        if (_registered)
            return;

        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreIfNullConvention(true),
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
        };

        ConventionRegistry.Register(
            ConventionPackName,
            pack,
            _ => true
        );

        _registered = true;
    }
}