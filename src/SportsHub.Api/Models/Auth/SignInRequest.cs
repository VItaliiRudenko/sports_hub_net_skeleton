using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

public class SignInRequest
{
    [Required]
    public SignInRequestUserModel Registration { get; set; }
}

public class SignInRequestUserModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}