using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces.Repositories;

namespace NotificationService.Application.Commands.Archive;

public class ArchiveCommandHandler(
    INotificationReadRepository notificationReadRepository,
    INotificationWriteRepository notificationWriteRepository,
    IArchiveRepository archiveRepository,
    ILogger<ArchiveCommandHandler> logger)
    : IRequestHandler<ArchiveCommand>
{
    public async Task Handle(ArchiveCommand request, CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow.AddDays(-request.DayCount); 

        try
        {
            var notifications = (await notificationReadRepository.GetNotificationsOlderThanDateAsync(date))
                .ToList();
            if (notifications.Count == 0)
                return;

            await archiveRepository.SaveRangeAsync(notifications);
            await notificationWriteRepository.DeleteNotificationAfterDateAsync(date);
            await notificationReadRepository.DeleteNotificationAfterDateAsync(date);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Data archiving error");
            throw;
        }
    }
}