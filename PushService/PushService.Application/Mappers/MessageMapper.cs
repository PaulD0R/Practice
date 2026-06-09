using PushService.Application.Events;
using PushService.Domain.Models;

namespace PushService.Application.Mappers;

public static class MessageMapper
{
    public static PushMessage ToPushMessage(this SendPushEvent message) =>
        new()
        {
            Id = message.MessageId,
            Name = message.Name,
            Email = message.Email,
            Body = message.Body,
            Subject = message.Subject,
        };

    public static ApprovePushEvent ToApprovePushEvent(this PushMessage message) =>
        new(message.Id);
}