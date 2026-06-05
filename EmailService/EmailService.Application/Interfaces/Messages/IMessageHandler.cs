namespace EmailService.Application.Interfaces.Messages;

public interface IMessageHandler<in TMessage>
{
    Task HandlerAsync(TMessage message, CancellationToken token = default);
}