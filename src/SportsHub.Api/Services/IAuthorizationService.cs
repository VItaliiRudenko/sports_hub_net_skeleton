using CSharpFunctionalExtensions;
using SportsHub.Api.Models.Auth;

namespace SportsHub.Api.Services;

public interface IAuthorizationService
{
    Task<Result<SignupResponse, string[]>> SignUp(SignupRequest signupRequest);
    Task<Result<SignInResponse>> SignIn(SignInRequest signInRequest);
    Task<Result> SignOut();
}