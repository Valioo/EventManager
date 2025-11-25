using EventManager.Application.Contracts;
using EventManager.Application.Requests.Authentication;
using EventManager.Application.Responses.Authentication;
using EventManager.Domain;
using EventManager.Domain.Entities;
using EventManager.Domain.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EventManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _hasher;


    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
        _hasher = new PasswordHasher<User>();
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken))
            return null!;

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = PasswordHashing.HashPassword(dto.Password)
        };

        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync();

        return await GenerateJwt(user, cancellationToken);
    }


    public async Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);

        if (user is null)
            return null!;

        var isAuthenticated = PasswordHashing.VerifyPassword(dto.Password, user.PasswordHash);

        if (!isAuthenticated)
            return null!;

        return await GenerateJwt(user, cancellationToken);
    }


    private async Task<AuthResultDto> GenerateJwt(User user, CancellationToken cancellationToken)
    {
        var roles = await _db.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            Subject = new ClaimsIdentity(claims),
            IssuedAt = null,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours(5),
            SigningCredentials = creds
        };

        var handler = new JsonWebTokenHandler();
        handler.SetDefaultTimesOnTokenCreation = false;
        var tokenString = handler.CreateToken(descriptor);

        return new AuthResultDto
        {
            Token = tokenString,
            Email = user.Email,
            Roles = roles
        };
    }
}
