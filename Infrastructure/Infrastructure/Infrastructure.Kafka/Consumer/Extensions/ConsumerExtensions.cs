using Infrastructure.Kafka.Common.Exceptions;

namespace Infrastructure.Kafka.Consumer.Extensions;

public static class ConsumerExtensions
{
    public static IKafkaConsumer GetByIdentifier(
        this IEnumerable<IKafkaConsumer> consumers,
        string identifier
    )
    {
        return consumers.FirstOrDefault(c => c.Identifier == identifier)
               ?? throw new ConsumerException($"No consumer with the identifier {identifier} was found.");
    }
}