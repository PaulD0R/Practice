using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationSolution.MessageBroker.Abstraction;
using RabbitMQ.Client;

namespace NotificationSolution.MessageBroker.Rabbit.Producer;

public class RabbitMessageProducer<TMessage> : IMessageProducer<TMessage>, IDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly RabbitProducerOptions _options;
    private readonly ILogger<RabbitMessageProducer<TMessage>> _logger;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMessageProducer(
        IOptionsMonitor<RabbitProducerOptions> options, 
        ILogger<RabbitMessageProducer<TMessage>> logger)
    {
        _options = options.Get(typeof(TMessage).Name);
        _factory = new ConnectionFactory
        {
            HostName = _options.HostName ?? throw new NullReferenceException("Host name is required"),
            UserName = _options.UserName ?? throw new NullReferenceException("UserName is required"),
            Password = _options.Password ??  throw new NullReferenceException("Password is required"),
        };
        _logger = logger;
    }
    
    public async Task ProduceAsync(TMessage message, CancellationToken token = default, TimeSpan? timeout = null)
    {
        try
        {
            await EnsureChannelInitializedAsync(token);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true 
            };
            
            if (timeout.HasValue && timeout.Value > TimeSpan.Zero)
            {
                properties.Headers = new Dictionary<string, object?>
                {
                    { "x-delay", (int)timeout.Value.TotalMilliseconds }
                };
            }

            await _channel!.BasicPublishAsync(
                exchange:  string.Empty,
                routingKey: _options.RoutingKey ?? throw new NullReferenceException("Routing key is required"),
                mandatory: false,
                basicProperties: properties, 
                body: body,
                cancellationToken: token);

            _logger.LogInformation("Message {Name} has been produced to RabbitMQ", typeof(TMessage).Name);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Error producing message to RabbitMQ");
        }
    }

    private async Task EnsureChannelInitializedAsync(CancellationToken token)
    {
        if (_channel != null) return;

        await _connectionLock.WaitAsync(token);
        try
        {
            if (_channel == null)
            {
                _connection = await _factory.CreateConnectionAsync(token);
                _channel = await _connection.CreateChannelAsync(cancellationToken: token);
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        _connectionLock.Dispose();
    }
}