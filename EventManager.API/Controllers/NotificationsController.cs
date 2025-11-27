using EventManager.Application.Contracts;
using EventManager.Application.Requests.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IEventNotificationService _eventNotificationService;

    public NotificationsController(IEventNotificationService eventNotificationService)
    {
        _eventNotificationService = eventNotificationService;
    }
    
    /// <summary>
    /// Creates a notification. Allowed for Administrators only
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateNotification(CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _eventNotificationService.CreateNotification(request, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Delets a notification. Allowed for Administrators only
    /// </summary>
    /// <param name="notificationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteNotification(int notificationId, CancellationToken cancellationToken)
    {
        var result = await _eventNotificationService.DeleteNotification(notificationId, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// List all notifications
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _eventNotificationService.GetEventNotifications(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Attach notification to event
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("events")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> AttachNotification([FromBody] EventNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _eventNotificationService.AttachNotification(request, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    /// <summary>
    /// Remove notification from event
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("events")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> DetachNotification([FromBody] EventNotificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _eventNotificationService.DetachNotification(request, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}
