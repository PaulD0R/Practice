using NotificationService.Application.Commands.AddNotification;
using NotificationService.Application.DTOs;
using NotificationService.Application.Events;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Mappers;

public static class NotificationMapper
{
    public static Notification ToNotification(this AddNotificationCommand command) =>
        new()
        {
            Address = command.Address,
            Text = command.Text,
            Channel = command.Channel,
            CreatedOn = DateTime.UtcNow
        };

    public static Notification ToNotification(this SyncAddNotificationCommand command) =>
        new()
        {
            Id = command.NotificationId,
            Address = command.Address,
            Text = command.Text,
            Status = command.Status,
            Channel = command.Channel,
            CreatedOn = command.CreatedOn
        };
    
    public static SyncAddNotificationCommand ToSyncAddNotificationCommand(this CreateNotificationEvent @event) =>
        new(@event.NotificationId, @event.Address, @event.Text,  @event.Status,  @event.Channel, @event.CreatedOn);

    extension(Notification notification)
    {
        public SendEmailEvent ToSendEmailEvent() =>
            new(notification.Id, notification.Address, notification.Text);

        public SendSmsEvent ToSendSmsEvent() =>
            new(notification.Id, notification.Address, notification.Text);

        public SendPushEvent ToSendPushEvent() =>
            new(notification.Id, notification.Address, notification.Text);

        public NotificationDto ToNotificationDto() =>
            new(notification.Id, notification.Address, notification.Text, notification.Status);
        
        public CreateNotificationEvent ToCreateNotificationEvent() =>
            new(notification.Id, notification.Address, notification.Text, notification.Status, notification.Channel, notification.CreatedOn);
    }
}