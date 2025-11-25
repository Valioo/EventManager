namespace EventManager.Application.Contracts;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    List<string> Roles { get; }
}
   