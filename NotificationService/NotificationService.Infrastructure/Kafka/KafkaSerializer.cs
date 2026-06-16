using System.Text.Json;
using Confluent.Kafka;

namespace NotificationService.Infrastructure.Kafka;

public class KafkaSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}