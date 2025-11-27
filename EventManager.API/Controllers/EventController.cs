using EventManager.Application.Contracts;
using EventManager.Application.Helpers.Pagination;
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

    /// <summary>
    /// Get single event details by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Create event. Allowed to Administrators & Organizers
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Update single event details. Allowed to Administrators & Organizers
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Soft delete an event. Allowed to Administrators & Organizers
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get all ticket types for a specified event
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attach a tag to event. Allowed to Administrators & Organizers 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="tagId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{eventId}/tags/{tagId}")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> AttachTagToEvent(int eventId, int tagId, CancellationToken cancellationToken)
    {
        var result = await _eventService.AttachTag(eventId, tagId, cancellationToken);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Remove a tag from event. Allowed to Administrators & Organizers
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="tagId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{eventId}/tags/{tagId}")]
    [Authorize(Policy = "AdminOrOrganizer")]
    public async Task<IActionResult> DeleteTagFromEvent(int eventId, int tagId, CancellationToken cancellationToken)
    {
        var result = await _eventService.DeleteTagFromEvent(eventId, tagId, cancellationToken);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Search for events based on criteria
    /// </summary>
    /// <param name="pages"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] PaginationQuery pages,[FromQuery] EventSearchRequest request, CancellationToken cancellationToken)
    {
        var response = await _eventService.Search(request, pages, cancellationToken);

        return Ok(response);
    }
}
