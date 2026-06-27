using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces.Repositories;

public interface INotificationReadRepository
{
    Task<IEnumerable<Notification>> GetNotificationsAsync();
    Task<Notification?> GetNotificationByIdAsync(Guid id);
    Task<IEnumerable<Notification>> GetNotificationsOlderThanDateAsync(DateTime date);
    Task<IEnumerable<Notification>> GetAllNotificationsAsync();
    Task AddNotificationAsync(Notification notification);
    Task DeleteNotificationAfterDateAsync(DateTime date);
}