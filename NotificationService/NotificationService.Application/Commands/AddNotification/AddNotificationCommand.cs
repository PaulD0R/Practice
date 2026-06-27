using System.ComponentModel.DataAnnotations;
using MediatR;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Commands.AddNotification;

public record AddNotificationCommand(
    [Required] string Address,
    [Required] string Text,
    [Required] NotificationChannel Channel) 
    : IRequest;