using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Context;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationReadRepository(ReadDbContext context) : INotificationReadRepository
{
    public async Task<IEnumerable<Notification>> GetNotificationsAsync()
    {
        return await context.Notifications.ToListAsync();
    }

    public async Task<Notification?> GetNotificationByIdAsync(Guid id)
    {
        return await context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetNotificationsOlderThanDateAsync(DateTime date)
    {
        return await context.Notifications.Where(n => n.CreatedOn < date).ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
    {
        return await context.Notifications.ToListAsync();
    }

    public async Task AddNotificationAsync(Notification notification)
    {
        await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();
    }

    public async Task DeleteNotificationAfterDateAsync(DateTime date)
    {
        await context.Notifications.Where(n => n.CreatedOn < date).ExecuteDeleteAsync();
    }
}