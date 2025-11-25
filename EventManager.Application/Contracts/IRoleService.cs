using EventManager.Application.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Application.Contracts;

public interface IRoleService
{
    public Task<IList<RoleResponseDto>> GetAllRoles(CancellationToken cancellationToken);
    public Task<bool> AssignRole(int roleId, int userId, CancellationToken cancellationToken);
    public Task<bool> UnassignRole(int roleId, int userId, CancellationToken cancellationToken);
}
