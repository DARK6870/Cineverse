using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Infrastructure.Kafka.Common.Constants;
using Infrastructure.Kafka.Common.Models.Metadata;

namespace Infrastructure.Kafka.Common.Extensions;

internal static class HeaderExtensions
{
    private static readonly string ServiceName;
    private static readonly string ServiceVersion;

    static HeaderExtensions()
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName();

        ServiceName = assemblyName.Name ?? "UnknownService";
        ServiceVersion = assemblyName.Version?.ToString() ?? "unknown";
    }
    
    internal static void AddInfrastructureMetadata<T>(this Message<string?, string> message)
    {
        message.Headers ??= [];

        var producedBy = new ProducedBy
        {
            Name = ServiceName,
            Version = ServiceVersion
        };
        
        message.Headers.Add(HeaderConstants.MessageType, Encoding.UTF8.GetBytes(typeof(T).FullName!));
        message.Headers.Add(HeaderConstants.ProducedBy, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(producedBy)));
        message.Headers.Add(HeaderConstants.TraceId, Encoding.UTF8.GetBytes(Activity.Current?.TraceId.ToString() ?? string.Empty));
    }
    
    internal static T GetRequiredJsonHeader<T>(
        this Headers headers,
        string headerKey
    )
    {
        if (!TryGetHeader(headers, headerKey, out var value))
            throw new InvalidOperationException($"Required kafka header '{headerKey}' is missing.");

        try
        {
            return JsonSerializer.Deserialize<T>(value)
                   ?? throw new InvalidOperationException($"Kafka header '{headerKey}' deserialized to null.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to deserialize kafka header '{headerKey}' to type '{typeof(T).Name}'.", ex);
        }
    }
    
    internal static string GetRequiredStringHeader(
        this Headers headers,
        string key
    )
    {
        if (!TryGetHeader(headers, key, out var value))
            throw new InvalidOperationException($"Required Kafka header '{key}' is missing.");

        return value;
    }

    private static bool TryGetHeader(
        this Headers headers,
        string key,
        out string value
    )
    {
        var header = headers.LastOrDefault(h => h.Key == key);
        
        if (header is null)
        {
            value = string.Empty;
            return false;
        }

        value = Encoding.UTF8.GetString(header.GetValueBytes());
        
        return true;
    }
}