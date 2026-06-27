using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs;

public record NotificationDto(Guid NotificationId, string Address, string Message, NotificationStatus Status);