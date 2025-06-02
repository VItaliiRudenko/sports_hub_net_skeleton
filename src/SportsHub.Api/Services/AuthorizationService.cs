using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SportsHub.Api.Models.Auth;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;
using SportsHub.Infrastructure.Db;

namespace SportsHub.Api.Services;

internal class AuthorizationService : IAuthorizationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizationService> _logger;
    private readonly IJwtDenyListRepository _denyListRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public AuthorizationService(
        UserManager<IdentityUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthorizationService> logger,
        IJwtDenyListRepository denyListRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _denyListRepository = denyListRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result<SignupResponse, string[]>> SignUp(SignupRequest signupRequest)
    {
        if (!string.Equals(signupRequest.Registration.Password, signupRequest.Registration.PasswordConfirmation))
        {
            return Result.Failure<SignupResponse, string[]>(["Password does not match with confirmation"]);
        }

        var identityUser = new IdentityUser(signupRequest.Registration.Email)
        {
            Email = signupRequest.Registration.Email,
        };

        var result = await _userManager.CreateAsync(identityUser, signupRequest.Registration.Password);
        if (!result.Succeeded)
        {
            return Result.Failure<SignupResponse, string[]>(result.Errors.Select(x => x.Description).ToArray());
        }

        var user = await _userManager.FindByEmailAsync(signupRequest.Registration.Email);
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

        var user = await _userManager.FindByEmailAsync(signInRequest.Registration.Email);
        if (user is null)
        {
            return defaultFailure;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, signInRequest.Registration.Password);
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

    public async Task<Result> SignOut()
    {
        var token = GetCurrentToken();
        if (token?.Id is null)
        {
            return Result.Failure("Can't sign out invalid token");
        }

        await _denyListRepository.DeleteExpired(DateTime.UtcNow);

        var denyRecord = new JwtDenyRecord
        {
            Jti = token.Id,
            Iat = token.IssuedAt,
            Exp = token.ValidTo,
        };

        _denyListRepository.Create(denyRecord);
        
        await _unitOfWork.CommitCurrentAsync();

        return Result.Success();
    }

    public async Task<bool> IsTokenInDenyList()
    {
        var token = GetCurrentToken();
        if (token is null)
        {
            return false;
        }

        var denyListITem = await _denyListRepository.GetById(token.Id);
        return (denyListITem is not null);
    }

    private JwtSecurityToken GetCurrentToken()
    {
        var authHeader = _httpContextAccessor.HttpContext?.Request?.Headers?.Authorization;
        if (authHeader is null)
        {
            return null;
        }

        var splitValues = authHeader.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (splitValues.Length != 2 ||
            !string.Equals(splitValues[0], "BEARER", StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrWhiteSpace(splitValues[1]))
        {
            return null;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(splitValues[1]);
            return token;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while parsing token");
            return null;
        }
    }

    public async Task<Result> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Return success even if user doesn't exist to prevent email enumeration
            return Result.Success();
        }

        // Check if there's an existing valid token
        var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "PasswordReset");
        if (!string.IsNullOrEmpty(existingToken))
        {
            return Result.Failure("A password reset request was already sent. Please wait 15 minutes before requesting another one.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.SetAuthenticationTokenAsync(user, "Default", "PasswordReset", token);

        try
        {
            await _emailService.SendPasswordResetEmailAsync(email, token);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
            return Result.Failure("Failed to send password reset email. Please try again later.");
        }
    }

    public async Task<Result> ResetPassword(ResetPasswordRequest request)
    {
        if (!string.Equals(request.Password, request.PasswordConfirmation))
        {
            return Result.Failure("Password does not match with confirmation");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure("Invalid request");
        }

        var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "PasswordReset");
        if (string.IsNullOrEmpty(storedToken) || storedToken != request.Token)
        {
            return Result.Failure("Invalid or expired token");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            return Result.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Remove the used token
        await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "PasswordReset");

        return Result.Success();
    }
}