using System.Text.Json;
using Infrastructure.Common.Json.Configuration;

namespace Infrastructure.Common.Helpers;

public static class JsonHelper
{
    /// <summary>
    /// Deserialize json object using default configuration
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerConfiguration.GetDefault())
            ?? throw new NullReferenceException($"Could not deserialize JSON string: {json}");
    }
    
    /// <summary>
    /// Serialize object using default configuration
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize(object? obj)
    {
        return obj == null ? string.Empty : JsonSerializer.Serialize(obj, JsonSerializerConfiguration.GetDefault());
    }
}