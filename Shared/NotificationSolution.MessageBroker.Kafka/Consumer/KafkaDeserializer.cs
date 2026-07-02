using System.Text.Json;
using Confluent.Kafka;

namespace NotificationSolution.MessageBroker.Kafka.Consumer;

public class KafkaDeserializer<TMessage> : IDeserializer<TMessage>
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) =>
        JsonSerializer.Deserialize<TMessage>(data) ?? throw new NullReferenceException();
}