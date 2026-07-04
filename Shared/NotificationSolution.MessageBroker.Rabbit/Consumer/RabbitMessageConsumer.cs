using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationSolution.MessageBroker.Abstraction;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationSolution.MessageBroker.Rabbit.Consumer;

public class RabbitMessageConsumer<TMessage> : BackgroundService
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IMessageHandler<TMessage> _handler;
    private readonly ILogger<RabbitMessageConsumer<TMessage>> _logger;
    private readonly string _queueName;
    
    // Храним ссылки для корректного закрытия в Dispose
    private IConnection? _connection;
    private IChannel? _channel; 

    public RabbitMessageConsumer(
        IOptionsMonitor<RabbitConsumerOptions> options,
        IMessageHandler<TMessage> handler,
        ILogger<RabbitMessageConsumer<TMessage>> logger)
    {
        var currentOptions = options.Get(typeof(TMessage).Name);
        _queueName = currentOptions.QueueName ?? throw new ArgumentNullException(nameof(currentOptions.QueueName));
        
        _connectionFactory = new ConnectionFactory
        {
            HostName = currentOptions.HostName ?? throw new ArgumentNullException(nameof(currentOptions.HostName)),
            UserName = currentOptions.UserName ?? throw new ArgumentNullException(nameof(currentOptions.UserName)),
            Password = currentOptions.Password ??  throw new ArgumentNullException(nameof(currentOptions.Password)),
        };

        _handler = handler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ consumer started for {Name}", typeof(TMessage).Name);

        _connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await _channel.QueueDeclareAsync(
            queue: _queueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<TMessage>(messageString);

                if (message != null)
                {
                    await _handler.HandleAsync(message, stoppingToken);
                    _logger.LogInformation("Message handled: {Message}", typeof(TMessage).Name);
                }
                
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while consuming message");
                
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken); 
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}