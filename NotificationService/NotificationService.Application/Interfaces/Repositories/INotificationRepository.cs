using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<Notification?> GetNotificationAsync(Guid id);
    Task<IEnumerable<Notification>> GetNotificationsOlderThanDateAsync(DateTime date);
    Task<Notification> AddAsync(Notification notification);
    Task<IEnumerable<Notification>> GetAllNotificationsAsync();
    Task<bool> UpdateStatusAsync(Guid id, NotificationStatus? emailStatus, NotificationStatus? smsStatus, NotificationStatus? pushStatus);
    Task DeleteNotificationAfterDateAsync(DateTime date);
}