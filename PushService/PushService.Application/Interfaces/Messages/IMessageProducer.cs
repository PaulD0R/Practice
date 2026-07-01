namespace PushService.Application.Interfaces.Messages;

public interface IMessageProducer<in TMessage>
{
    Task ProduceAsync(TMessage message, CancellationToken token = default);
}