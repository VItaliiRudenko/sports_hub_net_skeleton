using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

public class SignInRequest
{
    public SignInRequestUserModel User { get; set; }
}

public class SignInRequestUserModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}