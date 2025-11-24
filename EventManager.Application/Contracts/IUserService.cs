using EventManager.Application.Helpers.Pagination;
using EventManager.Application.Requests.Users;
using EventManager.Application.Responses.Users;

namespace EventManager.Application.Contracts;

public interface IUserService
{
    public Task<PaginatedResponse<UserResponseDto>> Get(PaginationQuery request);
    public Task<UserResponseDto> GetById(int id);
    public Task<bool> Delete(int id);
    public Task<UserResponseDto> Update(UpdateUserRequest request);
}
