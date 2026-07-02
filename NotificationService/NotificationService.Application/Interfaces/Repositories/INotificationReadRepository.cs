using NotificationService.Application.DTOs;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces.Repositories;

public interface INotificationReadRepository
{
    Task<IEnumerable<Notification>> GetNotificationsAsync(HelpNotificationDto helpNotification);
    Task<Notification?> GetNotificationByIdAsync(Guid id);
    Task AddNotificationAsync(Notification notification);
    Task DeleteNotificationAfterDateAsync(DateTime date);
    Task UpdateStatusAsync(Guid id, NotificationStatus status);
}