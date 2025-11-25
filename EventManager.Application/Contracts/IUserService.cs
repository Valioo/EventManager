using EventManager.Application.Helpers.Pagination;
using EventManager.Application.Requests.Users;
using EventManager.Application.Responses.Users;

namespace EventManager.Application.Contracts;

public interface IUserService
{
    public Task<PaginatedResponse<UserResponseDto>> Get(PaginationQuery request, CancellationToken cancellationToken);
    public Task<UserResponseDto> GetById(int id, CancellationToken cancellationToken);
    public Task<bool> Delete(int id, CancellationToken cancellationToken);
    public Task<UserResponseDto> Update(UpdateUserRequest request, CancellationToken cancellationToken);
}
