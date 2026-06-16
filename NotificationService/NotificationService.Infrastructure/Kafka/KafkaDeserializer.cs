using System.Text.Json;
using Confluent.Kafka;

namespace NotificationService.Infrastructure.Kafka;

public class KafkaDeserializer<TMessage> : IDeserializer<TMessage>
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) =>
        JsonSerializer.Deserialize<TMessage>(data) ?? throw new NullReferenceException();
}