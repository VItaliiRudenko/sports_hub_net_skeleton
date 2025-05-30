using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string PasswordConfirmation { get; set; }
} 