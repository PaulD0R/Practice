using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs;

/// <summary>
/// Данные об уведомлении.
/// </summary>
/// <param name="NotificationId">Идентификатор уведомления.</param>
/// <param name="Address">Адрес уведомления.</param>
/// <param name="Message">Текст уведомления.</param>
/// <param name="CreatedOn">Дата отправки уведомления.</param>
/// <param name="Status">Текущий статус отправки.</param>
public record NotificationDto(Guid NotificationId, string Address, string Message, DateTime CreatedOn, NotificationStatus Status);