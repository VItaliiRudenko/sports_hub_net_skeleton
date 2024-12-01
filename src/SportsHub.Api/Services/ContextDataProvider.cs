using System.Security.Claims;
using SportsHub.Domain.Services;

namespace SportsHub.Api.Services;

public class ContextDataProvider : IContextDataProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContextDataProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        if (identity is null)
        {
            return null;
        }

        if (!identity.IsAuthenticated)
        {
            return null;
        }

        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId;
    }
}