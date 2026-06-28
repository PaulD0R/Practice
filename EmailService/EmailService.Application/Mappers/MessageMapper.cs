using EmailService.Application.Events;
using EmailService.Domain.Models;

namespace EmailService.Application.Mappers;

public static class MessageMapper
{
    public static EmailMessage ToEmailMessage(this SendEmailEvent message) =>
        new()
        {
            Id = message.NotificationId,
            Email = message.Address,
            Body = message.Text
        };

    public static ApproveEmailEvent ToApproveMessageEvent(this EmailMessage message) =>
        new(message.Id);
}