using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SportsHub.Api.Models.Auth;

namespace SportsHub.Api.Services;

internal class AuthorizationService : IAuthorizationService
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthorizationService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<SignupResponse, string[]>> SignUp(SignupRequest signupRequest)
    {
        if (!string.Equals(signupRequest.User.Password, signupRequest.User.PasswordConfirmation))
        {
            return Result.Failure<SignupResponse, string[]>(["Password does not match with confirmation"]);
        }

        var identityUser = new IdentityUser(signupRequest.User.Email)
        {
            Email = signupRequest.User.Email,
        };

        var result = await _userManager.CreateAsync(identityUser, signupRequest.User.Password);
        if (!result.Succeeded)
        {
            return Result.Failure<SignupResponse, string[]>(result.Errors.Select(x => x.Description).ToArray());
        }

        var user = await _userManager.FindByEmailAsync(signupRequest.User.Email);
        if (user is null)
        {
            return Result.Failure<SignupResponse, string[]>(["Unexpected signup error"]);
        }

        return new SignupResponse
        {
            Id = user.Id,
            Email = user.Email,
        };
    }

    public async Task<Result<SignInResponse>> SignIn(SignInRequest signInRequest)
    {
        var defaultFailure = Result.Failure<SignInResponse>("Email or password is incorrect");

        var user = await _userManager.FindByEmailAsync(signInRequest.User.Email);
        if (user is null)
        {
            return defaultFailure;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, signInRequest.User.Password);
        if (!isPasswordValid)
        {
            return defaultFailure;
        }

        var handler = new JwtSecurityTokenHandler();

        var subject = new ClaimsIdentity(new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email),
        });

        var key = new SymmetricSecurityKey("SazsdfasgfdgfsdfSazsdfasgfdgfsdfSazsdfasgfdgfsdfSazsdfasgfdgfsdf"u8.ToArray());

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = "https://auth.sportshub.example.com",
            Audience = "https://app.sportshub.example.com",
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
        };

        var jwt = handler.CreateEncodedJwt(descriptor);

        return Result.Success(new SignInResponse
        {
            Id = user.Id,
            Email = user.Email,
            AuthenticationToken = jwt,
        });
    }

    public Task<Result> SignOut()
    {
        return Task.FromResult(Result.Success());
    }
}