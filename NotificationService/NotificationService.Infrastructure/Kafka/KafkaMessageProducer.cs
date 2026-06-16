using Confluent.Kafka;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Kafka;

public class KafkaMessageProducer<TMessage> : IMessageProducer<TMessage>, IDisposable
{
    private readonly IProducer<string, TMessage> _producer;
    private readonly string _topic;

    public KafkaMessageProducer(IOptionsMonitor<KafkaProducerOptions> options)
    {
        var currentOptions = options.Get(typeof(TMessage).Name);
        var config = new ProducerConfig
        {
            BootstrapServers = currentOptions.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, TMessage>(config).SetValueSerializer(new KafkaSerializer<TMessage>())
            .Build();
        _topic = currentOptions.Topic ?? throw new NullReferenceException();
    }
    
    public async Task ProduceAsync(TMessage message, CancellationToken token = default)
    {
        try
        {
            await _producer.ProduceAsync(_topic,  new Message<string, TMessage> {Value = message}, token);   
        }
        catch 
        {
            //
        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}