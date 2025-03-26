namespace SportsHub.Api.Models.Auth;

public class SignInResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string AuthenticationToken { get; set; }
}