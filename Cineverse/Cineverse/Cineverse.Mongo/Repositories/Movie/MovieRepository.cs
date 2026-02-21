using System.Linq.Expressions;
using System.Reflection;
using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Movie;

public class MovieRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<MovieEntity>(mongoDatabase), IMovieRepository
{
    public IQueryable<MovieEntity> SearchText(
        string searchText,
        params Expression<Func<MovieEntity, object>>[] excludedFields
    )
    {
        var query = Collection.AsQueryable();
        
        if (string.IsNullOrWhiteSpace(searchText))
            return query;

        var excludedPropertyNames = excludedFields
            .Select(GetPropertyName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var parameter = Expression.Parameter(typeof(MovieEntity), "x");
        var searchConstant = Expression.Constant(searchText.ToLower());
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

        var stringProperties = typeof(MovieEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string))
            .Where(p => !excludedPropertyNames.Contains(p.Name))
            .ToList();

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

        var lambda = Expression.Lambda<Func<MovieEntity, bool>>(combinedExpression, parameter);
        return query.Where(lambda);
    }
    
    private static string GetPropertyName(Expression<Func<MovieEntity, object>> expression)
    {
        return expression.Body switch
        {
            MemberExpression member => member.Member.Name,
            UnaryExpression { Operand: MemberExpression unaryMember } => unaryMember.Member.Name,
            _ => throw new ArgumentException("Invalid property expression")
        };
    }
}