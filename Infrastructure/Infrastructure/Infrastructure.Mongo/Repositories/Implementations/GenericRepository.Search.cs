using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Repositories.Implementations;

public partial class GenericRepository<T>
{
    public IQueryable<T> SearchByText(
        string searchText,
        params Expression<Func<T, object>>[] excludedFields
    )
    {
        var query = Collection.AsQueryable();

        if (string.IsNullOrWhiteSpace(searchText))
            return query;

        var alwaysExcluded = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            nameof(BaseEntity.Id)
        };

        if (typeof(TimestampedEntity).IsAssignableFrom(typeof(T)))
        {
            alwaysExcluded.Add(nameof(TimestampedEntity.DateCreated));
            alwaysExcluded.Add(nameof(TimestampedEntity.DateModified));
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var searchConstant = Expression.Constant(searchText.ToLower());
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;

        var excludedProperties = excludedFields
            .Select(e => e.Body switch
            {
                MemberExpression m => (PropertyInfo)m.Member,
                UnaryExpression { Operand: MemberExpression um } => (PropertyInfo)um.Member,
                _ => throw new ArgumentException("Invalid property expression")
            })
            .ToHashSet();

        var mappedIdProperties = BsonClassMap.LookupClassMap(typeof(T))
            .AllMemberMaps
            .Where(m => string.Equals(m.ElementName, "_id", StringComparison.OrdinalIgnoreCase))
            .Select(m => m.MemberInfo)
            .OfType<PropertyInfo>()
            .ToHashSet();

        var stringProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string))
            .Where(p => !alwaysExcluded.Contains(p.Name))
            .Where(p => p.GetCustomAttribute<BsonIdAttribute>() is null)
            .Where(p => !mappedIdProperties.Contains(p))
            .Where(p => !excludedProperties.Contains(p));

        Expression? combinedExpression = null;

        foreach (var property in stringProperties)
        {
            var propertyAccess = Expression.Property(parameter, property);
            var notNull = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));
            var toLower = Expression.Call(propertyAccess, toLowerMethod);
            var contains = Expression.Call(toLower, containsMethod, searchConstant);
            var safeContains = Expression.AndAlso(notNull, contains);

            combinedExpression = combinedExpression == null
                ? safeContains
                : Expression.OrElse(combinedExpression, safeContains);
        }

        if (combinedExpression == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        return query.Where(lambda);
    }
}
