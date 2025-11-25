using EventManager.Application.Contracts;
using EventManager.Application.Responses.Users;
using EventManager.Domain;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class RoleService : IRoleService
{
    private readonly AppDbContext _appDbContext;
    private readonly ICurrentUserService _currentUser;

    public RoleService(AppDbContext appDbContext, ICurrentUserService currentUser)
    {
        _appDbContext = appDbContext;
        _currentUser = currentUser;
    }
    public async Task<bool> AssignRole(int roleId, int userId, CancellationToken cancellationToken)
    {
        var role = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
        var user = await _appDbContext.Users
                            .Include(x => x.UserRoles)
                            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (role is null || user is null || user.UserRoles.Any(x => x.RoleId == roleId))
        {
            return false;
        }

        var userRole = new UserRole(userId, roleId);

        await _appDbContext.UserRoles.AddAsync(userRole, cancellationToken);
        await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IList<RoleResponseDto>> GetAllRoles(CancellationToken cancellationToken)
    {
        return await _appDbContext.Roles
                        .Select(x => new RoleResponseDto(x))
                        .ToListAsync(cancellationToken);
    }

    public async Task<bool> UnassignRole(int roleId, int userId, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId == userId)
        {
            return false;
        }

        var user = await _appDbContext.Users
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null || !user.UserRoles.Any(x => x.RoleId == roleId))
        {
            return false;
        }

        var userRole = user.UserRoles.First(x => x.RoleId == roleId);

        _appDbContext.UserRoles.Remove(userRole);
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
