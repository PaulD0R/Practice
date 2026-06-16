using Confluent.Kafka;
using EmailService.Application.Interfaces.Messages;
using EmailService.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace EmailService.Infrastructure.KafkaProducers;

public class KafkaMessageProducer<TMessage> : IMessageProducer<TMessage>, IDisposable
{
    private readonly IProducer<string, TMessage> _producer;
    private readonly string _topic;

    public KafkaMessageProducer(IOptionsMonitor<KafkaProducerOptions> options)
    {
        var currentOptions = options.Get(typeof(TMessage).Name);    
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = currentOptions.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, TMessage>(producerConfig)
            .SetValueSerializer(new KafkaSerializer<TMessage>()).Build();
        _topic = currentOptions.Topic;
    }
    
    public async Task ProduceAsync(TMessage message, CancellationToken token = default)
    {
        try
        {
            await _producer.ProduceAsync(_topic, new Message<string, TMessage> {Value = message}, token);
        }
        catch
        {
            
        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}