using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Auth;
using IAuthorizationService = SportsHub.Api.Services.IAuthorizationService;

namespace SportsHub.Api.Controllers;

/// <summary>
/// Controller for authentication and user management in the Sports Hub application
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthorizationService _authService;

    public AuthController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user in the system
    /// </summary>
    /// <param name="signupRequest">The signup information for the new user</param>
    /// <returns>The newly created user information</returns>
    /// <response code="200">Returns the newly created user</response>
    /// <response code="400">If the signup information is invalid</response>
    [HttpPost("/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// Authenticates a user and provides a JWT token
    /// </summary>
    /// <param name="signInRequest">The user credentials</param>
    /// <returns>Authentication token and user information</returns>
    /// <response code="200">Returns the authentication token and user information</response>
    /// <response code="400">If the credentials are invalid</response>
    [HttpPost("sign_in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// Signs out the current user by invalidating their token
    /// </summary>
    /// <returns>A success message</returns>
    /// <response code="200">Returns a success message</response>
    /// <response code="400">If the sign out process fails</response>
    /// <response code="401">If the user is not authenticated</response>
    [Authorize]
    [HttpDelete("sign_out")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// Initiates the password reset process for a user
    /// </summary>
    /// <param name="request">The forgot password request containing the user's email</param>
    /// <returns>A success message</returns>
    /// <response code="200">Returns a success message</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost("forgot_password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// Resets a user's password using a reset token
    /// </summary>
    /// <param name="request">The reset password request containing the token and new password</param>
    /// <returns>A success message</returns>
    /// <response code="200">Returns a success message</response>
    /// <response code="400">If the reset token is invalid or expired</response>
    [HttpPost("reset_password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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