namespace EventManager.Application.Responses.Authentication;

public class AuthResultDto
{
    public string Token { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
