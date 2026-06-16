using EmailService.Application.Events;
using EmailService.Application.Interfaces.Factories;
using EmailService.Application.Interfaces.Messages;
using EmailService.Application.Interfaces.Services;

namespace EmailService.API.KafkaConsumers.Handlers;

public class SendEmailEventHandler(IFactory<IEmailService> factory) : IMessageHandler<SendEmailEvent>
{
    public async Task HandlerAsync(SendEmailEvent message, CancellationToken token = default)
    {
        var service = factory.Create();
        var emailMessage = await service.SendAsync(message);
        await service.ApproveMessageAsync(emailMessage);
    }
}