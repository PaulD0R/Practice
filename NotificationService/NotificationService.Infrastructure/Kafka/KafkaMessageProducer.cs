using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Kafka;

public class KafkaMessageProducer<TMessage> : IMessageProducer<TMessage>, IDisposable
{
    private readonly IProducer<string, TMessage> _producer;
    private readonly string _topic;
    private readonly ILogger<KafkaMessageProducer<TMessage>> _logger;

    public KafkaMessageProducer(
        IOptionsMonitor<KafkaProducerOptions> options, 
        ILogger<KafkaMessageProducer<TMessage>> logger)
    {
        var currentOptions = options.Get(typeof(TMessage).Name);
        var config = new ProducerConfig
        {
            BootstrapServers = currentOptions.BootstrapServers,
        };
        _producer = new ProducerBuilder<string, TMessage>(config).SetValueSerializer(new KafkaSerializer<TMessage>())
            .Build();
        _topic = currentOptions.Topic ?? throw new NullReferenceException();
        _logger = logger;
    }
    
    public async Task ProduceAsync(TMessage message, CancellationToken token = default)
    {
        try
        {
            await _producer.ProduceAsync(_topic,  new Message<string, TMessage> {Value = message}, token);   
            _logger.LogInformation($"Message {message} has been produced");
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Error producing message");
        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}