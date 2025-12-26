namespace Infrastructure.Kafka.Exceptions;

public class ConsumerException(string message) : Exception(message);