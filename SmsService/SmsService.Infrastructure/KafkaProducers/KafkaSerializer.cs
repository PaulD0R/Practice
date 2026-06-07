using System.Text.Json;
using Confluent.Kafka;

namespace SmsService.Infrastructure.KafkaProducers;

public class KafkaSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}