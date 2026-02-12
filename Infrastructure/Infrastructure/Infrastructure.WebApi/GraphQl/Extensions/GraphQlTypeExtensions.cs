using System.Reflection;

namespace Infrastructure.WebApi.GraphQl.Extensions;

internal static class GraphQlTypeExtensions
{
    internal static Type[] GetGraphQlExtensions<T>(this Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => !t.IsAbstract
                        && t.IsSubclassOf(typeof(ObjectTypeExtension))
                        && t.BaseType?.GenericTypeArguments
                            .FirstOrDefault()?
                            .CustomAttributes
                            .Any(attr => attr.ConstructorArguments
                                .Any(arg => arg.Value is string value && value == typeof(T).Name)
                            ) == true
            )
            .ToArray();
    }
}