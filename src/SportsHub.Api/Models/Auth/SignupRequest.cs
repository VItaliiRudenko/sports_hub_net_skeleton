using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

public class SignupRequest
{
    public SignupRequestUserModel User { get; set; }
}

public class SignupRequestUserModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    
    [Required]
    public string PasswordConfirmation { get; set; }
}

