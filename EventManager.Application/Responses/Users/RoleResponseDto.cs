using EventManager.Domain.Entities;

namespace EventManager.Application.Responses.Users;

public class RoleResponseDto
{
    public int RoleId { get; set; }
    public string Name { get; set; }

    public RoleResponseDto(UserRole role)
    {
        RoleId = role.RoleId;
        Name = role.Role.Name;
    }

    public RoleResponseDto(Role role)
    {
        RoleId = role.Id;
        Name = role.Name;
    }
}
