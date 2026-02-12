using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Infrastructure.Common.Helpers;
using Infrastructure.Common.Json.Configuration;
using Infrastructure.Kafka.Constants;
using Infrastructure.Kafka.Models.Metadata;

namespace Infrastructure.Kafka.Extensions;

public static class ConsumeResultExtensions
{
    /// <summary>
    /// Get deserialized message
    /// </summary>
    /// <param name="consumeResult"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T GetMessageValue<T>(this ConsumeResult<string?, string> consumeResult)
    {
        return JsonHelper.Deserialize<T>(consumeResult.Message.Value);
    }
    
    internal static InfrastructureMessageMetadata GetInfrastructureMetadata<TKey, TValue>(
        this ConsumeResult<TKey, TValue> consumeResult
    )
    {
        if (consumeResult?.Message?.Headers is null)
            throw new InvalidOperationException("Kafka message headers are missing.");

        var headers = consumeResult.Message.Headers;

        var producedBy = GetRequiredJsonHeader<ProducedBy>(
            headers,
            HeaderConstants.ProducedBy
        );

        var messageType = GetRequiredStringHeader(
            headers,
            HeaderConstants.MessageType
        );

        var traceContext = ExtractTraceContext(headers);

        return new InfrastructureMessageMetadata
        {
            ProducedBy = producedBy,
            MessageType = messageType,
            SerializedTraceContext = traceContext
        };
    }

    private static string GetRequiredStringHeader(
        Headers headers,
        string key)
    {
        if (!TryGetHeader(headers, key, out var value))
            throw new InvalidOperationException($"Required Kafka header '{key}' is missing.");

        return value;
    }

    private static bool TryGetHeader(
        Headers headers,
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

    private static Dictionary<string, string> ExtractTraceContext(
        Headers headers
    )
    {
        var result = new Dictionary<string, string>();

        foreach (var header in headers)
        {
            if (header.Key.StartsWith("trace", StringComparison.OrdinalIgnoreCase))
            {
                result[header.Key] = Encoding.UTF8.GetString(header.GetValueBytes());
            }
        }

        return result;
    }

    private static T GetRequiredJsonHeader<T>(
        Headers headers,
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

}
