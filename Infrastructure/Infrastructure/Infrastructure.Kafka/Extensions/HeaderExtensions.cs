using System.Reflection;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Infrastructure.Kafka.Constants;
using Infrastructure.Kafka.Models.Metadata;

namespace Infrastructure.Kafka.Extensions;

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
    
    internal static Message<string?, string> AddInfrastructureMetadata<T>(
        this Message<string?, string> message
    )
    {
        message.Headers ??= [];

        message.Headers.Add(
            HeaderConstants.MessageType,
            Encoding.UTF8.GetBytes(typeof(T).FullName!)
        );

        var producedBy = new ProducedByComponent
        {
            Name = ServiceName,
            Version = ServiceVersion
        };

        message.Headers.Add(
            HeaderConstants.ProducedBy,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(producedBy))
        );

        return message;
    }
}