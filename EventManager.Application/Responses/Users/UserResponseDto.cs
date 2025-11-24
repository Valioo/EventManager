using EventManager.Domain.Entities;
using System.Collections.Immutable;

namespace EventManager.Application.Responses.Users;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public IImmutableList<RoleResponseDto> Roles { get; set; }

    public UserResponseDto(User user)
    {
        Id = user.Id;
        FullName = user.FullName;
        Email = user.Email;
        if (user.UserRoles is not null && user.UserRoles.Count != 0)
        {
            Roles = [.. user.UserRoles.Select(x => new RoleResponseDto(x))];
        }
        else
        {
            Roles = [];
        }
    }
}
