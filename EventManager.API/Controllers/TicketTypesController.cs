using EventManager.Application.Contracts;
using EventManager.Application.Requests.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "AdminOrOrganizer")]
[ApiController]
public class TicketTypesController : ControllerBase
{
    private readonly ITicketTypeService _ticketTypeService;

    public TicketTypesController(ITicketTypeService ticketTypeService)
    {
        _ticketTypeService = ticketTypeService;
    }

    /// <summary>
    /// Create a ticket type for an event. Allowed to Administrators & Organizers
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _ticketTypeService.CreateTicketType(request, cancellationToken);

        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    /// <summary>
    /// Update ticket-type (only capacity). Allowed to Administrators & Organizers
    /// </summary>
    /// <param name="request"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateTicketTypeRequest request, int id, CancellationToken cancellationToken)
    {
        var result = await _ticketTypeService.UpdateTicketType(request, id, cancellationToken);

        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}
