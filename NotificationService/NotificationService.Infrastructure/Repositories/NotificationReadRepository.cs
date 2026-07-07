using Microsoft.EntityFrameworkCore;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Context;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationReadRepository(ReadDbContext context) : INotificationReadRepository
{
    public async Task<IEnumerable<Notification>> GetNotificationsAsync(HelpNotificationDto helpNotification)
    {
        var query = context.Notifications.AsQueryable().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(helpNotification.Address))
            query = query.Where(n => n.Address == helpNotification.Address);
        
        if (!helpNotification.Date.HasValue) return await query.ToListAsync();

        var rawDate = helpNotification.Date.Value.ToDateTime(TimeOnly.MinValue);
        var startDate = DateTime.SpecifyKind(rawDate, DateTimeKind.Utc);
        var endDate = startDate.AddDays(1);

        query = query.Where(n => n.CreatedOn >= startDate && n.CreatedOn < endDate);
        return await query.ToListAsync();
    }

    public async Task<Notification?> GetNotificationByIdAsync(Guid id)
    {
        return await context.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
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

    public async Task UpdateStatusAsync(Guid id, NotificationStatus status)
    {
        await context.Notifications.Where(n => n.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(n => n.Status, status));
    }
}