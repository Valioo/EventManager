using EventManager.Application.Contracts;
using EventManager.Application.Helpers.Pagination;
using EventManager.Application.Requests.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationQuery request, CancellationToken cancellationToken)
    {
        return Ok(await _userService.Get(request, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _userService.GetById(id, cancellationToken);
        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _userService.Delete(id, cancellationToken);

        if (!response)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _userService.Update(request, cancellationToken);

        if (response is null)
        {
            return BadRequest();
        }

        return Ok(response);
    }
}
