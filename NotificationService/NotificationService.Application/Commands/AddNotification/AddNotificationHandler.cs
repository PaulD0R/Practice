using System.Text.RegularExpressions;
using MediatR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Events;
using NotificationService.Application.Events.Email;
using NotificationService.Application.Events.Push;
using NotificationService.Application.Events.Sms;
using NotificationService.Application.Interfaces.Messages;
using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Application.Mappers;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Commands.AddNotification;

public class AddNotificationHandler(
    INotificationWriteRepository notificationWriteRepository,
    IMessageProducer<SendEmailEvent> emailProducer,
    IMessageProducer<SendSmsEvent> smsProducer,
    IMessageProducer<SendPushEvent> pushProducer,
    IMessageProducer<CreateNotificationEvent> createNotificationProducer,
    ILogger<AddNotificationHandler> logger)
    : IRequestHandler<AddNotificationCommand>
{
    public async Task Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        Notification notification;
        try
        {
            notification = await notificationWriteRepository.AddAsync(request.ToNotification());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving notification: {message}", e.Message);
            throw new InternalServerException("Error saving notification");
        }
        
        switch (request.Channel)
        {
            case NotificationChannel.Email:
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(request.Address, emailPattern))
                    throw new BadRequestException("Invalid email");
                break;
            case NotificationChannel.Sms:
                var phonePattern = @"^\+\d{1,4}[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$";
                if (!Regex.IsMatch(request.Address, phonePattern))
                    throw new BadRequestException("Invalid phone");
                break;
            case NotificationChannel.Push:
                break;
            default:
                throw new BadRequestException("Invalid channel");
        }
        
        var status = NotificationStatus.Sent;
        try
        {
            switch (notification.Channel)
            {
                case NotificationChannel.Email:
                    await emailProducer.ProduceAsync(notification.ToSendEmailEvent(), cancellationToken);
                    break;
                case NotificationChannel.Sms:
                    await smsProducer.ProduceAsync(notification.ToSendSmsEvent(), cancellationToken);
                    break;
                case NotificationChannel.Push:
                    await pushProducer.ProduceAsync(notification.ToSendPushEvent(), cancellationToken);
                    break;
            }
        }
        catch(Exception e)
        {
            logger.LogError(e, "Error sending notification {NotificationId}", notification.Id);
            status = NotificationStatus.Failed;
        }

        try
        {
            await notificationWriteRepository.UpdateStatusAsync(notification.Id, status);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Status not updated for notification {NotificationId}", notification.Id);
            throw new InternalServerException("Messages sent but status not updated for notification");
        }
        finally
        {
            await createNotificationProducer.ProduceAsync(notification.ToCreateNotificationEvent(), cancellationToken);
        }
    }
}