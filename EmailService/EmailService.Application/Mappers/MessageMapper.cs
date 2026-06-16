using EmailService.Application.Events;
using EmailService.Domain.Models;

namespace EmailService.Application.Mappers;

public static class MessageMapper
{
    public static EmailMessage ToEmailMessage(this SendEmailEvent message) =>
        new()
        {
            Id = message.MessageId,
            Name = message.Name,
            Email = message.Email,
            Body = message.Body,
            Subject = message.Subject,
        };

    public static ApproveEmailEvent ToApproveMessageEvent(this EmailMessage message) =>
        new(message.Id);
}