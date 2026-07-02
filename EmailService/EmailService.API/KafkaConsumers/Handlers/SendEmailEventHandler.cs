using EmailService.Application.Events;
using EmailService.Application.Interfaces.Services;
using NotificationSolution.MessageBroker.Abstraction;

namespace EmailService.API.KafkaConsumers.Handlers;

public class SendEmailEventHandler(IServiceScopeFactory scopeFactory) : IMessageHandler<SendEmailEvent>
{
    public async Task HandleAsync(SendEmailEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IEmailService>();
        try
        {
            var emailMessage = await service.SendEmailAsync(message);
            await service.SendApproveMessageAsync(emailMessage);
        }
        catch(Exception e)
        {
            await service.SendErrorMessageAsync(message.NotificationId, e.Message);
        }
    }
}