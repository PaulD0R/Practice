using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces.Repositories;

public interface IArchiveRepository
{
    Task SaveRangeAsync(IEnumerable<Notification> notifications);
}