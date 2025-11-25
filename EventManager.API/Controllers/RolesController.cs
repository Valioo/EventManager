using EventManager.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _roleService.GetAllRoles(cancellationToken));
    }

    [HttpPost("{roleId}/assign/{userId}")]
    public async Task<IActionResult> Assign(int roleId, int userId, CancellationToken cancellationToken)
    {
        var result = await _roleService.AssignRole(roleId, userId, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("{roleId}/unassign/{userId}")]
    public async Task<IActionResult> Unassign(int roleId, int userId, CancellationToken cancellationToken)
    {
        var result = await _roleService.UnassignRole(roleId, userId, cancellationToken);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}
