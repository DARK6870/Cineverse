using System.Text.Json;
using Confluent.Kafka;
using Infrastructure.Common.Json.Configuration;

namespace NotificationService.Application.Helpers;

public static class KafkaHelper
{
    public static T GetMessageValue<T>(this ConsumeResult<string?, string> consumeResult)
    {
        var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value, JsonSerializerConfiguration.JsonSerializerOptions)
            ?? throw new Exception("Unable to deserialize the message");
        
        return message;
    }
}