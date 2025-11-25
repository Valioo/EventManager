using EventManager.Application.Contracts;
using EventManager.Application.Requests.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;


    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto, CancellationToken cancellationToken)
    {
        var result = await _auth.RegisterAsync(dto, cancellationToken);

        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }


    [HttpPost("login")] // Username = admin@gmail.com, Password = admin
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _auth.LoginAsync(dto, cancellationToken);

        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}
