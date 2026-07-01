using Microsoft.Extensions.Logging;
using PushService.Application.Events;
using PushService.Application.Interfaces.Messages;
using PushService.Application.Interfaces.Services;
using PushService.Application.Mappers;
using PushService.Domain.Exceptions;
using PushService.Domain.Models;

namespace PushService.Application.Services;

public class PushService(
    IPushSender sender,
    IMessageProducer<ApprovePushEvent> approveProducer,
    IMessageProducer<ErrorPushEvent> errorProducer,
    ILogger<PushService> logeer)
    : IPushService
{
    public async Task<PushMessage> SendPushAsync(SendPushEvent message)
    {
        var pushMessage = message.ToPushMessage();
        try
        {
            await sender.SendAsync(pushMessage);
            return pushMessage;
        }
        catch (Exception e)
        {
            logeer.LogError(e, "Error sending push message: {message}", e.Message);
            throw new InternalServerException("Error sending push message: " + e.Message);
        }
    }

    public async Task SendApproveMessageAsync(PushMessage message)
    {
        var approveEvent = message.ToApprovePushEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }

    public async Task SendErrorMessageAsync(Guid notificationId, string message)
    {
        await errorProducer.ProduceAsync(new ErrorPushEvent(notificationId, message));
    }
}
