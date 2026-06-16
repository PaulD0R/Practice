using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Events;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Application.Interfaces.Services;

namespace NotificationService.Infrastructure.Kafka.Handlers;

public class ErrorEventHandler<TMessage>(
    IServiceScopeFactory scopeFactory, 
    ILogger<ErrorEventHandler> logger)
    : IMessageHandler<TMessage> where TMessage : ErrorEvent
{
    public async Task HandleAsync(TMessage message, CancellationToken token = default)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<INotificationService>();

            await service.SetChannelStatusFailedAsync(message.NotificationId, message.Channel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update error status for notification {NotificationId} in ErrorEventHandler<{EventType}>", 
                message.NotificationId, typeof(TMessage).Name);
            throw; 
        }
    }
}