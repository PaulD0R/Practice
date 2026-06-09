using PushService.Application.Events;
using PushService.Application.Interfaces.Messages;
using PushService.Application.Interfaces.Services;
using PushService.Application.Mappers;
using PushService.Domain.Models;

namespace PushService.Application.Services;

public class PushService(
    IPushSender sender,
    IMessageProducer<ApprovePushEvent> approveProducer)
    : IPushService
{
    public async Task<PushMessage> SendAsync(SendPushEvent message)
    {
        var pushMessage = message.ToPushMessage();
        await sender.SendAsync(pushMessage);
        return pushMessage;
    }

    public async Task ApproveMessageAsync(PushMessage message)
    {
        var approveEvent = message.ToApprovePushEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }
}
