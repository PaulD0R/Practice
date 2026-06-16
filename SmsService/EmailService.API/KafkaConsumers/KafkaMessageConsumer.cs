using Confluent.Kafka;
using EmailService.API.Options;
using Microsoft.Extensions.Options;
using SmsService.Application.Interfaces.Messages;

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
        var config = new ConsumerConfig
        {
            BootstrapServers = currentOptions.BootstrapServers,
            GroupId = currentOptions.GroupId
        };
        
        _consumer = new ConsumerBuilder<string, TMessage>(config)
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
                await _messageHandler.HandleAsync(message.Message.Value, stoppingToken);
            }
            catch
            {
                    
            }
        }
        
        _consumer.Close();  
    }
}