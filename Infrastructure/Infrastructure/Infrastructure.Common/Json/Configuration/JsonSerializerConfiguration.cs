using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Common.Json.Configuration
{
    public static class JsonSerializerConfiguration
    {
        /// <summary>
        /// Returns a fresh instance of JsonSerializerOptions for safe per-use serialization/deserialization.
        /// Avoids "read-only or already used" exceptions.
        /// </summary>
        public static JsonSerializerOptions GetDefault()
        {
            return new JsonSerializerOptions
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
    }
}