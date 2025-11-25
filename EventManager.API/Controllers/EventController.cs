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
    private readonly ITicketTypeService _ticketTypeService;

    public EventController(IEventService eventService, ITicketTypeService ticketTypeService)
    {
        _eventService = eventService;
        _ticketTypeService = ticketTypeService;
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

    [HttpGet("{id}/ticket-types")]
    public async Task<IActionResult> GetEventTicketTypes(int id, CancellationToken cancellationToken)
    {
        var ticketTypes = await _ticketTypeService.GetTicketTypesByEvent(id, cancellationToken);

        if (ticketTypes is null)
        {
            return BadRequest();
        }

        return Ok(ticketTypes);
    }

    [HttpPost("{eventId}/tags/{tagId}")]
    public async Task<IActionResult> AttachTagToEvent(int eventId, int tagId, CancellationToken cancellationToken)
    {
        var result = await _eventService.AttachTag(eventId, tagId, cancellationToken);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{eventId}/tags/{tagId}")]
    public async Task<IActionResult> DeleteTagFromEvent(int eventId, int tagId, CancellationToken cancellationToken)
    {
        var result = await _eventService.DeleteTagFromEvent(eventId, tagId, cancellationToken);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}
