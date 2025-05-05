using Microsoft.Extensions.DependencyInjection;
using SportsHub.Api.Models.Auth;
using SportsHub.Api.Services;
using SportsHub.Infrastructure.Db;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SportsHub.Api.IntegrationTests;

internal static class SystemUnderTest
{
    private static CustomWebApplicationFactory _factory;
    public static IServiceScope GlobalScope { get; private set; }
    public static AppDbContext DbContext { get; private set; }
    public static IApplicationMapper Mapper { get; private set; }

    public static JsonSerializerOptions DefaultJsonOptions { get; private set; } = new() {PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower};

    public static void Initialize()
    {
        _factory = new CustomWebApplicationFactory();

        GlobalScope = _factory.Services.CreateScope();
        DbContext = GlobalScope.ServiceProvider.GetRequiredService<AppDbContext>();
        Mapper = GlobalScope.ServiceProvider.GetRequiredService<IApplicationMapper>();
    }

    public static void Dispose()
    {
        GlobalScope.Dispose();
        _factory.Dispose();
    }

    public static HttpClient GetAnonymousHttpClient() => _factory.CreateClient();

    public static async Task<HttpClient> GetAuthorizedHttpClient()
    {
        var httpClient = _factory.CreateClient();
        

        var response = await httpClient.PostAsJsonAsync("/api/auth/sign_in", new SignInRequest
        {
            User = new SignInRequestUserModel
            {
                Email = "test1@gmail.com",
                Password = "password1",
            },
        }, DefaultJsonOptions);

        var signInResponse = await response.Content.ReadFromJsonAsync<SignInResponse>(DefaultJsonOptions);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInResponse.AuthenticationToken);
        return httpClient;
    }
}