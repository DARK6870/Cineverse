namespace Infrastructure.Kafka.Common.Exceptions;

public class ProducerException(string message) : Exception(message);