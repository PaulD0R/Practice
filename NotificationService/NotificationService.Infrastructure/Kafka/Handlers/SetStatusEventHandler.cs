using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Commands.SetStatusNotification;
using NotificationService.Application.Events;
using NotificationSolution.MessageBroker.Abstraction;

namespace NotificationService.Infrastructure.Kafka.Handlers;

public class SetStatusEventHandler(
    IServiceScopeFactory scopeFactory, 
    ILogger<SetStatusEventHandler> logger)
    : IMessageHandler<SetStatusEvent>
{
    public async Task HandleAsync(SetStatusEvent message, CancellationToken token = default)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SyncSetStatusNotificationCommand(message.NotificationId, message.Status), token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to sync status for notification {NotificationId}", message.NotificationId);
            throw; 
        }
    }
}