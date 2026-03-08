using Infrastructure.Kafka.Common.Exceptions;

namespace Infrastructure.Kafka.Producer.Extensions;

public static class ProducerExtensions
{
    public static IKafkaProducer GetByIdentifier(
        this IEnumerable<IKafkaProducer> producers,
        string identifier
    )
    {
        return producers.SingleOrDefault(c => c.Identifier == identifier)
               ?? throw new ProducerException($"No producer with the identifier {identifier} was found.");
    }
}