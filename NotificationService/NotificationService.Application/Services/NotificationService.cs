using Microsoft.Extensions.Logging;
using NotificationService.Application.DTOs;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Application.Interfaces.Services;
using NotificationService.Application.Mappers;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    IArchiveRepository archiveRepository,
    IMessageProducer<SendEmailEvent> emailProducer,
    IMessageProducer<SendSmsEvent> smsProducer,
    IMessageProducer<SendPushEvent> pushProducer,
    ILogger<NotificationService> logger)
    : INotificationService
{
    public async Task SendNotificationAsync(NotificationRequest request)
    {
        Notification notification;
        try
        {
            notification = await notificationRepository.AddAsync(request.ToNotification());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving notification: {message}", ex.Message);
            throw new InternalServerException("Error saving notification");
        }

        var emailTask = notification.Email != null
            ? TrySendAsync("Email", () => emailProducer.ProduceAsync(notification.ToSendEmailEvent()), notification.Id)
            : Task.FromResult<NotificationStatus?>(null);

        var smsTask = notification.Phone != null
            ? TrySendAsync("SMS", () => smsProducer.ProduceAsync(notification.ToSendSmsEvent()), notification.Id)
            : Task.FromResult<NotificationStatus?>(null);

        var pushTask = notification.Push != null
            ? TrySendAsync("Push", () => pushProducer.ProduceAsync(notification.ToSendPushEvent()), notification.Id)
            : Task.FromResult<NotificationStatus?>(null);

        var results = await Task.WhenAll(emailTask, smsTask, pushTask);

        try
        {
            await notificationRepository.UpdateStatusAsync(notification.Id, results[0], results[1], results[2]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Messages sent but status not updated for notification {NotificationId}",
                notification.Id);
            throw new InternalServerException("Messages sent but status not updated for notification");
        }

        return;

        async Task<NotificationStatus?> TrySendAsync(string channel, Func<Task> producerAction, Guid id)
        {
            try
            {
                await producerAction();
                return NotificationStatus.Sent;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending notification {NotificationId} via channel {NotificationChannel}", id, channel);
                return NotificationStatus.Failed;
            }
        }
    }

    public async Task ArchiveAsync(int dayCount)
    {
        var date = DateTime.UtcNow.AddDays(-dayCount);

        try
        {
            var notifications = (await notificationRepository.GetNotificationsOlderThanDateAsync(date)).ToList();
            if (notifications.Count == 0)
                return;

            await archiveRepository.SaveRangeAsync(notifications);
            await notificationRepository.DeleteNotificationAfterDateAsync(date);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Data archiving error");
            throw;
        }
    }

    public async Task SetChannelStatusFailedAsync(Guid id, NotificationChannel channel)
    {
        var emailStatus = channel == NotificationChannel.Email ? NotificationStatus.Failed : (NotificationStatus?)null;
        var smsStatus = channel == NotificationChannel.Sms ? NotificationStatus.Failed : (NotificationStatus?)null;
        var pushStatus = channel == NotificationChannel.Push ? NotificationStatus.Failed : (NotificationStatus?)null;

        try
        {
            await notificationRepository.UpdateStatusAsync(id, emailStatus, smsStatus, pushStatus);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update status in database for notification {NotificationId} on channel {Channel}.", 
                id, channel);
        
            throw; 
        }
    }
}