using Confluent.Kafka;
using EmailService.API.Options;
using EmailService.Application.Interfaces.Messages;
using Microsoft.Extensions.Options;

namespace EmailService.API.KafkaConsumers;

public class KafkaMessageConsumer<TMessage> : BackgroundService
{
    private readonly IConsumer<string, TMessage> _consumer;    
    private readonly IMessageHandler<TMessage> _messageHandler;

    public KafkaMessageConsumer(
        IOptionsMonitor<KafkaConsumerOptions> options, 
        IMessageHandler<TMessage> messageHandler)
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
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(stoppingToken);
                await _messageHandler.HandlerAsync(message.Message.Value, stoppingToken);
            }
            catch
            {
                
            }
        }
        
        _consumer.Close();
    }
}