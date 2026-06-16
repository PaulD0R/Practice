using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Context;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    public async Task<Notification?> GetNotificationAsync(Guid id)
    {
        return await context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
    }

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

    public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
    {
        return await context.Notifications.ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(
        Guid id, 
        NotificationStatus? emailStatus, 
        NotificationStatus? smsStatus, 
        NotificationStatus? pushStatus)
    {
        return await context.Notifications.Where(n => n.Id == id)
            .ExecuteUpdateAsync(setters => 
        {
            if (emailStatus.HasValue)
                setters.SetProperty(n => n.EmailStatus, emailStatus);

            if (smsStatus.HasValue)
                setters.SetProperty(n => n.SmsStatus, smsStatus);

            if (pushStatus.HasValue)
                setters.SetProperty(n => n.PushStatus, pushStatus);
        }) > 0;
    }

    public async Task DeleteNotificationAfterDateAsync(DateTime date)
    {
        await context.Notifications.Where(n => n.CreatedOn < date).ExecuteDeleteAsync();
    }
}