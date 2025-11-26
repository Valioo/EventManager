using EventManager.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class EventSubscriptionsController : ControllerBase
{
    private readonly IEventSubscriptionService _eventSubscriptionService;

    public EventSubscriptionsController(IEventSubscriptionService eventSubscriptionService)
    {
        _eventSubscriptionService = eventSubscriptionService;
    }

    /// <summary>
    /// Get all event subscriptions
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _eventSubscriptionService.GetAll(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Subscribes to an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Subscribe([FromBody] int eventId, CancellationToken cancellationToken)
    {
        var result = await _eventSubscriptionService.SubscribeToEvent(eventId, cancellationToken);

        if(!result)
        {
            return BadRequest();
        }

        return Ok();
    }


    /// <summary>
    /// Unsubscribes from an event
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Unsubscribe([FromBody] int eventId, CancellationToken cancellationToken)
    {
        var result = await _eventSubscriptionService.UnsubscribeFromEvent(eventId, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}
