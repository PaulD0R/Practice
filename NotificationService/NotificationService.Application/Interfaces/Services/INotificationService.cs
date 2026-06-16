using NotificationService.Application.DTOs;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationRequest request);
    Task ArchiveAsync(int dayCount);
    Task SetChannelStatusFailedAsync(Guid id, NotificationChannel channel);
}