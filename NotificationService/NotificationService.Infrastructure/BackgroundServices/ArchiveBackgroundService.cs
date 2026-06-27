using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Commands.Archive;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.BackgroundServices;

public class ArchiveBackgroundService(
    IServiceScopeFactory scopeFactory, 
    IOptionsMonitor<ArchiveOptions> options,
    ILogger<ArchiveBackgroundService> logger) 
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ArchiveBackgroundService is starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Starting archiving task");
                
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new ArchiveCommand(options.CurrentValue.DayCount), stoppingToken);
                
                logger.LogInformation("Daily archiving task finished successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Archiving execution error");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        logger.LogInformation("ArchiveBackgroundService is stopped");
    }
}