using PushService.Application.Events;
using PushService.Domain.Models;

namespace PushService.Application.Mappers;

public static class MessageMapper
{
    public static PushMessage ToPushMessage(this SendPushEvent message) =>
        new()
        {
            Id = message.NotificationId,
            Address = message.Address,
            Body = message.Text,
        };

    public static ApprovePushEvent ToApprovePushEvent(this PushMessage message) =>
        new(message.Id);
}