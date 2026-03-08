using Confluent.Kafka;
using Infrastructure.Common.Helpers;
using Infrastructure.Kafka.Common.Constants;
using Infrastructure.Kafka.Common.Models.Metadata;

namespace Infrastructure.Kafka.Common.Extensions;

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
        if (consumeResult.Message?.Headers is null)
            throw new InvalidOperationException("Kafka message headers are missing.");

        var headers = consumeResult.Message.Headers;

        return new InfrastructureMessageMetadata
        {
            ProducedBy = headers.GetRequiredJsonHeader<ProducedBy>(HeaderConstants.ProducedBy),
            MessageType = headers.GetRequiredStringHeader(HeaderConstants.MessageType),
            TraceId = headers.GetRequiredStringHeader(HeaderConstants.TraceId)
        };
    }
}
