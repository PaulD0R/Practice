using NotificationSolution.MessageBroker.Abstraction;
using PushService.Application.Events;
using PushService.Application.Interfaces.Services;

namespace PushService.API.KafkaConsumers.Handlers;

public class SendPushEventHandler(IServiceScopeFactory scopeFactory) : IMessageHandler<SendPushEvent>
{
    public async Task HandleAsync(SendPushEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPushService>();
        try
        {
            var pushMessage = await service.SendPushAsync(message);
            await service.SendApproveMessageAsync(pushMessage);
        }
        catch (Exception e)
        {
            await service.SendErrorMessageAsync(message.NotificationId, e.Message);
        }
    }
}