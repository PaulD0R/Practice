using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Kafka;

public class KafkaMessageConsumer<TMessage> : BackgroundService
{
    private readonly IConsumer<string, TMessage> _consumer;
    private readonly IMessageHandler<TMessage> _handler;
    private readonly ILogger<KafkaMessageConsumer<TMessage>> _logger;

    public KafkaMessageConsumer(
        IOptionsMonitor<KafkaConsumerOptions> options, 
        IMessageHandler<TMessage> handler,
        ILogger<KafkaMessageConsumer<TMessage>> logger)
    {
        var currentOptions = options.Get(typeof(TMessage).Name);
        var config = new ConsumerConfig
        {
            BootstrapServers = currentOptions.BootstrapServers,
            GroupId = currentOptions.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        _consumer = new ConsumerBuilder<string, TMessage>(config)
            .SetValueDeserializer(new KafkaDeserializer<TMessage>()).Build();
        _consumer.Subscribe(currentOptions.Topic);
        _handler = handler;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kafka consumer started {Name}", typeof(TMessage).Name);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(stoppingToken);
                await _handler.HandleAsync(message.Message.Value, stoppingToken);
                _logger.LogInformation("Message received: {Message}", typeof(TMessage).Name);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error occured while consuming message");
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}