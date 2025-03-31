using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;

namespace SportsHub.Api.Middlewares;

public class JwtDenyListMiddleware: IMiddleware
{
    private readonly Services.IAuthorizationService _authorizationService;

    public JwtDenyListMiddleware(Services.IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        var endpointMetadata = context.Features?.Get<IEndpointFeature>()?.Endpoint?.Metadata;
        if (endpointMetadata == null)
        {
            await next(context);
            return;
        }

        var hasAuthorizeAttribute = endpointMetadata.Any(m => m is AuthorizeAttribute);
        if (!hasAuthorizeAttribute)
        {
            await next(context);
            return;
        }

        var isInDenyList = await _authorizationService.IsTokenInDenyList();
        if (isInDenyList)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(context);
    }
}