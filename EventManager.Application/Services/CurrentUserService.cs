using EventManager.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace EventManager.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    public int? UserId { get; }
    public string? Email { get; }
    public List<string> Roles { get; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;//{http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: 
        if (user?.Identity?.IsAuthenticated == true)
        {
            UserId = int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (int?)null;
            Email = user.FindFirstValue(ClaimTypes.Email);
            Roles = [.. user.FindAll(ClaimTypes.Role).Select(r => r.Value)];
        }
        else
        {
            Roles = [];
        }
    }
}
