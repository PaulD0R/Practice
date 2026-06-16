using NotificationService.Application.DTOs;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Mappers;

public static class NotificationMapper
{
    public static Notification ToNotification(this NotificationRequest request) =>
        new()
        {
            Email = request.Email,
            Phone = request.Phone,
            Subject = request.Subject,
            Text = request.Text,
            CreatedOn = DateTime.UtcNow
        };

    extension(Notification notification)
    {
        public SendEmailEvent ToSendEmailEvent() =>
            new(notification.Email ?? throw new NullReferenceException(),
                notification.Subject ?? string.Empty,
                notification.Text);

        public SendSmsEvent ToSendSmsEvent() =>
            new(notification.Text);

        public SendPushEvent ToSendPushEvent() =>
            new(notification.Text);
    }
}