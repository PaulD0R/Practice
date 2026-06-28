using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Context;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationWriteRepository(WriteDbContext context) : INotificationWriteRepository
{ 
    public async Task<IEnumerable<Notification>> GetNotificationsOlderThanDateAsync(DateTime date)
    {
        return await context.Notifications.Where(n => n.CreatedOn < date).ToListAsync();
    }
    
    public async Task<Notification> AddAsync(Notification notification)
    {
        var newNotification = await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();
        return newNotification.Entity;
    }

    public async Task<bool> UpdateStatusAsync(Guid id, NotificationStatus status)
    {
        return await context.Notifications.Where(n => n.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(n => n.Status, status)) > 0;
    }

    public async Task DeleteNotificationAfterDateAsync(DateTime date)
    {
        await context.Notifications.Where(n => n.CreatedOn < date).ExecuteDeleteAsync();
    }
}