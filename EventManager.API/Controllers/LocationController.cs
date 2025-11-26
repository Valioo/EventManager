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

    /// <summary>
    /// List all locations
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _locationService.GetLocations(cancellationToken));
    }

    /// <summary>
    /// Creates a location. Allowed to Administrators only
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates a location. Allowed to Administrators only
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Deletes a location. This is not a soft delete. Allowed to Administrators only
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
