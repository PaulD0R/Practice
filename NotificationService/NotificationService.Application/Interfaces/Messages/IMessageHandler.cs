namespace NotificationService.Application.Interfaces.Messages;

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken token = default);
}