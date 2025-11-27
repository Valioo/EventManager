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

    /// <summary>
    /// List all users. Allowed to Adminisrtators only
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationQuery request, CancellationToken cancellationToken)
    {
        return Ok(await _userService.Get(request, cancellationToken));
    }

    /// <summary>
    /// Get user by id. Allowed to Adminisrtators only
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Soft delete user. Allowed to Adminisrtators only
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Update user info. Allowed to Adminisrtators only
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
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
