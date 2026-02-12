using HotChocolate.Types.Descriptors;

namespace Infrastructure.WebApi.GraphQl.NamingConventions;

public class ApplicationNamingConvention : DefaultNamingConventions
{
    public override string GetEnumValueName(object value) => value.ToString() ?? string.Empty;
}