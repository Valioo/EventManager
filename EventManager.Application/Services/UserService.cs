using EventManager.Application.Contracts;
using EventManager.Application.Helpers.Pagination;
using EventManager.Application.Requests.Users;
using EventManager.Application.Responses.Users;
using EventManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> Delete(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            return false;
        }

        user.IsDeleted = true;
        await _dbContext.SaveChangesAsync();

        return user.IsDeleted;
    }

    public async Task<PaginatedResponse<UserResponseDto>> Get(PaginationQuery request)
    {
        var totalCount = await _dbContext.Users.CountAsync();
        var maximumPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        if (request.PageNumber > maximumPages)
        {
            return new PaginatedResponse<UserResponseDto>
            {
                Result = [],
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                MaximumPages = maximumPages
            };
        }

        var users = await _dbContext.Users
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role)
                            .OrderBy(u => u.FullName)
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToListAsync();

        var result = users.Select(u => new UserResponseDto(u)).ToList();

        return new PaginatedResponse<UserResponseDto>
        {
            Result = result,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            MaximumPages = maximumPages
        };
    }

    public async Task<UserResponseDto> GetById(int id)
    {
        var user = await _dbContext.Users
            .Include(x => x.UserRoles)
                .ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            return null!;
        }

        return new UserResponseDto(user);
    }

    public async Task<UserResponseDto> Update(UpdateUserRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId);

        bool attemptDatabaseSave = false;

        if (user is null)
        {
            return null!;
        }

        if (request.FullName is not null)
        {
            user.FullName = request.FullName;
            attemptDatabaseSave = true;
        }

        if (request.Email is not null)
        {
            user.Email = request.Email;
            attemptDatabaseSave = true;
        }

        if (attemptDatabaseSave)
        {
            await _dbContext.SaveChangesAsync();
        }

        return new UserResponseDto(user);
    }
}
