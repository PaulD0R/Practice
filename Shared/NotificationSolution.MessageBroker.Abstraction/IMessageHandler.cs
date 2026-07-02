namespace NotificationSolution.MessageBroker.Abstraction;

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken token = default);
}