namespace NotificationSolution.MessageBroker.Abstraction;

public interface IMessageProducer<in TMessage>
{
    Task ProduceAsync(TMessage message, CancellationToken token = default);
}