using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Auth;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
} 