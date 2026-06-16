using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces.Services;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/v1.0.0/notifications")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        await notificationService.SendNotificationAsync(request);
        return Ok();
    }
}