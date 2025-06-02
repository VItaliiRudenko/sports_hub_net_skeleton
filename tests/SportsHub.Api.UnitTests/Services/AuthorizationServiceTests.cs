using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SportsHub.Api.Models.Auth;
using SportsHub.Api.Services;
using SportsHub.Domain.Entities;
using SportsHub.Domain.Repositories;
using SportsHub.Infrastructure.Db;
using System.Security.Claims;

namespace SportsHub.Api.UnitTests.Services;

[TestFixture]
public class AuthorizationServiceTests
{
    private IAuthorizationService _sut;
    private UserManager<IdentityUser> _userManager;
    private IHttpContextAccessor _httpContextAccessor;
    private ILogger<AuthorizationService> _logger;
    private IJwtDenyListRepository _denyListRepository;
    private IUnitOfWork _unitOfWork;
    private IEmailService _emailService;

    [SetUp]
    public void SetUp()
    {
        _userManager = A.Fake<UserManager<IdentityUser>>(x => x.WithArgumentsForConstructor(() => 
            new UserManager<IdentityUser>(
                A.Fake<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null)));
        
        _httpContextAccessor = A.Fake<IHttpContextAccessor>();
        _logger = A.Fake<ILogger<AuthorizationService>>();
        _denyListRepository = A.Fake<IJwtDenyListRepository>();
        _unitOfWork = A.Fake<IUnitOfWork>();
        _emailService = A.Fake<IEmailService>();

        _sut = new AuthorizationService(
            _userManager,
            _httpContextAccessor,
            _logger,
            _denyListRepository,
            _unitOfWork,
            _emailService);
    }

    [Test]
    public async Task SignUp_PasswordMismatch_ReturnsFailure()
    {
        var signupRequest = new SignupRequest
        {
            Registration = new SignupRequestUserModel
            {
                Email = "test@example.com",
                Password = "password123",
                PasswordConfirmation = "different_password"
            }
        };

        var result = await _sut.SignUp(signupRequest);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Contains.Item("Password does not match with confirmation"));
    }

    [Test]
    public async Task SignUp_ValidRequest_CreatesUser()
    {
        var signupRequest = new SignupRequest
        {
            Registration = new SignupRequestUserModel
            {
                Email = "test@example.com",
                Password = "password123",
                PasswordConfirmation = "password123"
            }
        };

        var identityUser = new IdentityUser("test@example.com") { Email = "test@example.com", Id = "user-id" };
        
        A.CallTo(() => _userManager.CreateAsync(A<IdentityUser>._, A<string>._))
            .Returns(IdentityResult.Success);
        A.CallTo(() => _userManager.FindByEmailAsync("test@example.com"))
            .Returns(identityUser);

        var result = await _sut.SignUp(signupRequest);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Email, Is.EqualTo("test@example.com"));
        Assert.That(result.Value.Id, Is.EqualTo("user-id"));
    }

    [Test]
    public async Task SignUp_UserCreationFails_ReturnsFailure()
    {
        var signupRequest = new SignupRequest
        {
            Registration = new SignupRequestUserModel
            {
                Email = "test@example.com",
                Password = "password123",
                PasswordConfirmation = "password123"
            }
        };

        var identityErrors = new[] { new IdentityError { Description = "Password too weak" } };
        A.CallTo(() => _userManager.CreateAsync(A<IdentityUser>._, A<string>._))
            .Returns(IdentityResult.Failed(identityErrors));

        var result = await _sut.SignUp(signupRequest);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Contains.Item("Password too weak"));
    }

    [Test]
    public async Task SignIn_UserNotFound_ReturnsFailure()
    {
        var signInRequest = new SignInRequest
        {
            User = new SignInRequestUserModel
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            }
        };

        A.CallTo(() => _userManager.FindByEmailAsync("nonexistent@example.com"))
            .Returns((IdentityUser)null);

        var result = await _sut.SignIn(signInRequest);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo("Email or password is incorrect"));
    }

    [Test]
    public async Task SignIn_InvalidPassword_ReturnsFailure()
    {
        var signInRequest = new SignInRequest
        {
            User = new SignInRequestUserModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            }
        };

        var identityUser = new IdentityUser("test@example.com") { Email = "test@example.com", Id = "user-id" };
        
        A.CallTo(() => _userManager.FindByEmailAsync("test@example.com"))
            .Returns(identityUser);
        A.CallTo(() => _userManager.CheckPasswordAsync(identityUser, "wrongpassword"))
            .Returns(false);

        var result = await _sut.SignIn(signInRequest);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.EqualTo("Email or password is incorrect"));
    }

    [Test]
    public async Task SignIn_ValidCredentials_ReturnsToken()
    {
        var signInRequest = new SignInRequest
        {
            User = new SignInRequestUserModel
            {
                Email = "test@example.com",
                Password = "password123"
            }
        };

        var identityUser = new IdentityUser("test@example.com") { Email = "test@example.com", Id = "user-id" };
        
        A.CallTo(() => _userManager.FindByEmailAsync("test@example.com"))
            .Returns(identityUser);
        A.CallTo(() => _userManager.CheckPasswordAsync(identityUser, "password123"))
            .Returns(true);

        var result = await _sut.SignIn(signInRequest);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Email, Is.EqualTo("test@example.com"));
        Assert.That(result.Value.Id, Is.EqualTo("user-id"));
        Assert.That(result.Value.AuthenticationToken, Is.Not.Null);
        Assert.That(result.Value.AuthenticationToken, Is.Not.Empty);
    }

    [Test]
    public async Task ForgotPassword_UserNotFound_ReturnsSuccess()
    {
        A.CallTo(() => _userManager.FindByEmailAsync("nonexistent@example.com"))
            .Returns((IdentityUser)null);

        var result = await _sut.ForgotPassword("nonexistent@example.com");

        Assert.That(result.IsSuccess, Is.True);
        A.CallTo(() => _emailService.SendPasswordResetEmailAsync(A<string>._, A<string>._))
            .MustNotHaveHappened();
    }

    [Test]
    public async Task ForgotPassword_ValidUser_SendsEmail()
    {
        var identityUser = new IdentityUser("test@example.com") { Email = "test@example.com", Id = "user-id" };
        
        A.CallTo(() => _userManager.FindByEmailAsync("test@example.com"))
            .Returns(identityUser);
        A.CallTo(() => _userManager.GetAuthenticationTokenAsync(identityUser, "Default", "PasswordReset"))
            .Returns((string)null);
        A.CallTo(() => _userManager.GeneratePasswordResetTokenAsync(identityUser))
            .Returns("reset-token");

        var result = await _sut.ForgotPassword("test@example.com");

        Assert.That(result.IsSuccess, Is.True);
        A.CallTo(() => _emailService.SendPasswordResetEmailAsync("test@example.com", "reset-token"))
            .MustHaveHappened();
    }

    [Test]
    public async Task ForgotPassword_ExistingToken_ReturnsFailure()
    {
        var identityUser = new IdentityUser("test@example.com") { Email = "test@example.com", Id = "user-id" };
        
        A.CallTo(() => _userManager.FindByEmailAsync("test@example.com"))
            .Returns(identityUser);
        A.CallTo(() => _userManager.GetAuthenticationTokenAsync(identityUser, "Default", "PasswordReset"))
            .Returns("existing-token");

        var result = await _sut.ForgotPassword("test@example.com");

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Contains.Substring("already sent"));
    }
} 