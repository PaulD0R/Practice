using Confluent.Kafka;
using EmailService.API.Options;
using EmailService.Application.Interfaces.Messages;
using Microsoft.Extensions.Options;

namespace EmailService.API.KafkaConsumers;

public class KafkaMessageConsumer<TMessage> : BackgroundService
{
    private readonly IConsumer<string, TMessage> _consumer;    
    private readonly IMessageHandler<TMessage> _messageHandler;
    private readonly ILogger<KafkaMessageConsumer<TMessage>> _logger;

    public KafkaMessageConsumer(
        IOptionsMonitor<KafkaConsumerOptions> options, 
        IMessageHandler<TMessage> messageHandler,
        ILogger<KafkaMessageConsumer<TMessage>> logger)
    {
        var currentOptions = options.Get(typeof(TMessage).Name);
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = currentOptions.BootstrapServers,
            GroupId = currentOptions.GroupId
        };

        _consumer = new ConsumerBuilder<string, TMessage>(consumerConfig)
            .SetValueDeserializer(new KafkaDeserializer<TMessage>()).Build();
        _consumer.Subscribe(currentOptions.Topic);
        _messageHandler = messageHandler;
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
                await _messageHandler.HandlerAsync(message.Message.Value, stoppingToken);
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