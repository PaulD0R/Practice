using PushService.Application.Events;
using PushService.Application.Interfaces.Messages;
using PushService.Application.Interfaces.Services;

namespace PushService.API.KafkaConsumers.Handlers;

public class SendPushEventHandler(IServiceScopeFactory scopeFactory) : IMessageHandler<SendPushEvent>
{
    public async Task HandlerAsync(SendPushEvent message, CancellationToken token = default)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPushService>();
        var emailMessage = await service.SendAsync(message);
        await service.ApproveMessageAsync(emailMessage);
    }
}