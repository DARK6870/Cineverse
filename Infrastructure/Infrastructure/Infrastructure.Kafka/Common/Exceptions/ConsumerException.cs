namespace Infrastructure.Kafka.Common.Exceptions;

public class ConsumerException(string message) : Exception(message);