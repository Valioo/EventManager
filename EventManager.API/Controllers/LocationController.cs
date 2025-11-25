using EventManager.Application.Contracts;
using EventManager.Application.Requests.Categories;
using EventManager.Application.Requests.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _locationService.GetLocations(cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var response = await _locationService.CreateLocation(request, cancellationToken);

        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLocationRequest request, CancellationToken cancellationToken)
    {
        var response = await _locationService.UpdateLocation(request, id, cancellationToken);

        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _locationService.DeleteLocation(id, cancellationToken);

        if (!response)
        {
            return BadRequest();
        }

        return Ok();
    }
}
