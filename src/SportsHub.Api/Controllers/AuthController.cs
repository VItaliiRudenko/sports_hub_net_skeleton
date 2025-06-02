using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Auth;
using IAuthorizationService = SportsHub.Api.Services.IAuthorizationService;

namespace SportsHub.Api.Controllers;

/// <summary>
/// Authentication and user management endpoints
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly IAuthorizationService _authService;

    public AuthController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="signupRequest">User registration details</param>
    /// <returns>Registration confirmation and user details</returns>
    /// <response code="200">User successfully registered</response>
    /// <response code="400">Invalid registration data or email already exists</response>
    [HttpPost("/users")]
    [ProducesResponseType(typeof(SignupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignupRequest signupRequest)
    {
        var result = await _authService.SignUp(signupRequest);
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = result.Error });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Sign in an existing user
    /// </summary>
    /// <param name="signInRequest">User credentials</param>
    /// <returns>Authentication token and user information</returns>
    /// <response code="200">User successfully authenticated</response>
    /// <response code="400">Invalid credentials or authentication failed</response>
    [HttpPost("sign_in")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
    {
        var result = await _authService.SignIn(signInRequest);
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = new []{ result.Error } });
        }
        return Ok(result.Value);
    }

    /// <summary>
    /// Sign out the current authenticated user
    /// </summary>
    /// <returns>Sign out confirmation</returns>
    /// <response code="200">User successfully signed out</response>
    /// <response code="400">Sign out failed</response>
    /// <response code="401">User not authenticated</response>
    [Authorize]
    [HttpDelete("sign_out")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignOutCurrent()
    {
        var result = await _authService.SignOut();
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = new[] { result.Error } });
        }

        return Ok(new { Message = "Signed out successfully." });
    }

    /// <summary>
    /// Request password reset for a user account
    /// </summary>
    /// <param name="request">Email address for password reset</param>
    /// <returns>Password reset request confirmation</returns>
    /// <response code="200">Password reset email sent (if email exists)</response>
    /// <response code="400">Invalid email format or request failed</response>
    [HttpPost("forgot_password")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var result = await _authService.ForgotPassword(request.Email);
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = new[] { result.Error } });
        }

        return Ok(new { Message = "If your email is registered, you will receive a password reset link." });
    }

    /// <summary>
    /// Reset user password using reset token
    /// </summary>
    /// <param name="request">Password reset details including token and new password</param>
    /// <returns>Password reset confirmation</returns>
    /// <response code="200">Password successfully reset</response>
    /// <response code="400">Invalid reset token or request failed</response>
    [HttpPost("reset_password")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPassword(request);
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = new[] { result.Error } });
        }

        return Ok(new { Message = "Password has been reset successfully." });
    }
}