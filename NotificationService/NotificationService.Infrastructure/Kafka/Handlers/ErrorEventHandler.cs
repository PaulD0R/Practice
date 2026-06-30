using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Commands.SetStatusNotification;
using NotificationService.Application.Events;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Kafka.Handlers;

public class ErrorEventHandler(
    IServiceScopeFactory scopeFactory, 
    ILogger<ErrorEventHandler> logger)
    : IMessageHandler<ErrorEvent>
{
    public async Task HandleAsync(ErrorEvent message, CancellationToken token = default)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SetStatusNotificationCommand(message.NotificationId, NotificationStatus.Failed), token);
            logger.LogInformation("Failed to send notification {NotificationId}: {Message}",
                message.NotificationId, message.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to update error status for notification {NotificationId}", message.NotificationId);
            throw; 
        }
    }
}