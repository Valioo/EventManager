using EventManager.Application.Contracts;
using EventManager.Application.Requests.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase(PurchaseTicketRequest request, CancellationToken cancellationToken)
    {
        var response = await _ticketService.PurchaseTicket(request, cancellationToken);

        if (!response)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("mine")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _ticketService.GetMyTickets(cancellationToken);

        return Ok(response);
    }
}
