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
    
    public static RetrySendEmailEvent ToRetrySendEmailEvent(this SendEmailEvent message) =>
        new(message.NotificationId, message.Address, message.Text, 1);
    
    public static SendEmailEvent ToSendEmailEvent(this RetrySendEmailEvent message) =>
        new(message.NotificationId, message.Address, message.Text);
    
    public static RetrySendEmailEvent ToNewRetrySendEmailEvent(this RetrySendEmailEvent message) =>
        new(message.NotificationId, message.Address, message.Text, message.RetryNumber + 1);
}