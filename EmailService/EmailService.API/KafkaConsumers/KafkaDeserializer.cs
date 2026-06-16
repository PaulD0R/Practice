using System.Text.Json;
using Confluent.Kafka;

namespace EmailService.API.KafkaConsumers;

public class KafkaDeserializer<TMessage> : IDeserializer<TMessage>
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) =>
        JsonSerializer.Deserialize<TMessage>(data.ToArray()) ?? throw new NullReferenceException();
}