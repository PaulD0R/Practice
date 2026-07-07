namespace NotificationService.Application.DTOs;

/// <summary>
/// DTO параметр фильтрации уведомлений.
/// </summary>
/// <param name="Address">Адрес получателя (опционально).</param>
/// <param name="Date">Дата отправки уведомления (опционально).</param>
public record HelpNotificationDto(string? Address, DateOnly? Date);