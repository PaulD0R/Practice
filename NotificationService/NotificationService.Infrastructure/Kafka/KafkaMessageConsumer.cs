using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Kafka;

public class KafkaMessageConsumer<TMessage> : BackgroundService
{
    private readonly IConsumer<string, TMessage> _consumer;
    private readonly IMessageHandler<TMessage> _handler;

    public KafkaMessageConsumer(IOptionsMonitor<KafkaConsumerOptions> options, IMessageHandler<TMessage> handler)
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
        _handler = handler;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(stoppingToken);
                await _handler.HandleAsync(message.Message.Value, stoppingToken);
            }
            catch
            {
                //
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