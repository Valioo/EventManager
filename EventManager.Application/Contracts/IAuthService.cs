using EventManager.Application.Requests.Authentication;
using EventManager.Application.Responses.Authentication;

namespace EventManager.Application.Contracts;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken);
    Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
}
