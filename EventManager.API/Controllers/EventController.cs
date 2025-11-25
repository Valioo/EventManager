using EventManager.Application.Contracts;
using EventManager.Application.Requests.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(int id, CancellationToken cancellationToken)
    {
        var evt = await _eventService.GetById(id, cancellationToken);

        if (evt is null)
        {
            return BadRequest();
        }

        return Ok(evt);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        var createdEvent = await _eventService.CreateEvent(request, cancellationToken);

        if (createdEvent is null)
        {
            return BadRequest();
        }

        return Ok(createdEvent);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var updated = await _eventService.UpdateEvent(request, id, cancellationToken);

        if (updated is null)
        {
            return BadRequest();
        }

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
    {
        var deleted = await _eventService.DeleteEvent(id, cancellationToken);

        if (!deleted)
        {
            return BadRequest();
        }

        return Ok();
    }
}
