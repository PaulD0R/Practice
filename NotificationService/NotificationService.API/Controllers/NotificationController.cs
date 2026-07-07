using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NotificationService.Application.Commands.AddNotification;
using NotificationService.Application.DTOs;
using NotificationService.Application.Queries.GetNotifications;

namespace NotificationService.API.Controllers;

[ApiController]
//[Authorize]
[EnableRateLimiting("NotificationPolicy")]
[Route("api/notifications")]
public class NotificationController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Отправить новое уведомление.
    /// </summary>
    /// <param name="command">Данные для создания и отправки уведомления.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SendNotification([FromBody] AddNotificationCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        await mediator.Send(command);
        return Created();
    }

    /// <summary>
    /// Получить список уведомлений.
    /// </summary>
    /// <param name="helpNotification">Параметры фильтрации.</param>
    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] HelpNotificationDto helpNotification)
    {
        var notifications = await mediator.Send(new GetNotificationsQuery(helpNotification));
        return Ok(notifications);
    }
}