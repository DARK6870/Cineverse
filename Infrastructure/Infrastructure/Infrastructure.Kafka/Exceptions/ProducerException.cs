namespace Infrastructure.Kafka.Exceptions;

public class ProducerException(string message) : Exception(message);