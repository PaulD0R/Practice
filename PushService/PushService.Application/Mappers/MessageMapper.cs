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
    
    public static RetrySendPushEvent ToRetrySendPushEvent(this SendPushEvent message) =>
        new(message.NotificationId, message.Address, message.Text, 1);
    
    public static SendPushEvent ToSendPushEvent(this RetrySendPushEvent message) =>
        new(message.NotificationId, message.Address, message.Text);
    
    public static RetrySendPushEvent ToNewRetrySendPushEvent(this RetrySendPushEvent message) =>
        new(message.NotificationId, message.Address, message.Text, message.RetryNumber + 1);
}