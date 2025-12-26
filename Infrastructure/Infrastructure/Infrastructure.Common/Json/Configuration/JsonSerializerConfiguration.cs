using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Common.Json.Configuration;

public static class JsonSerializerConfiguration
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        IncludeFields = true,
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
}