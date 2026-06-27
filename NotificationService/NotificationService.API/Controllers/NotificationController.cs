using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NotificationService.Application.Commands.AddNotification;
using NotificationService.Application.Queries.GetNotifications;

namespace NotificationService.API.Controllers;

[ApiController]
[Authorize]
[EnableRateLimiting("NotificationPolicy")]
[Route("api/notifications")]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] AddNotificationCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        await mediator.Send(command);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var notifications = await mediator.Send(new GetNotificationsQuery());
        return Ok(notifications);
    }
}