using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces.Repositories;

public interface INotificationWriteRepository
{
    Task<Notification> AddAsync(Notification notification);
    Task<bool> UpdateStatusAsync(Guid id, NotificationStatus status);
    Task DeleteNotificationAfterDateAsync(DateTime date);
}