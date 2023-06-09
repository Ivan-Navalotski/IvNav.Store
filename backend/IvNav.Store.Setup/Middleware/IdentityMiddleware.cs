using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using IvNav.Store.Common.Constants;
using IvNav.Store.Common.Identity;

namespace IvNav.Store.Setup.Middleware;

/// <summary>
/// IdentityMiddleware
/// </summary>
public class IdentityMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="next"></param>
    public IdentityMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invoke
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        var userIdString = string.Empty;
        var tenantIdString = string.Empty;
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            userIdString = context.User.FindFirstValue(ClaimIdentityConstants.UserIdClaimType);
            tenantIdString = context.User.FindFirstValue(ClaimIdentityConstants.TenantIdClaimType);
        }
        else
        {
            if (context.Request.Headers.TryGetValue(HeaderNames.UserId, out var userIdHeaderValue))
            {
                userIdString = userIdHeaderValue.FirstOrDefault() ?? string.Empty;
            }
            if (context.Request.Headers.TryGetValue(HeaderNames.TenantId, out var tenantIdHeaderValue))
            {
                tenantIdString = tenantIdHeaderValue.FirstOrDefault() ?? string.Empty;
            }
        }

        if (Guid.TryParse(userIdString, out var userId))
        {
            IdentityState.SetCurrent(userId, Guid.TryParse(tenantIdString, out var tenantId) ? tenantId : null);
        }

        await _next(context);
    }
}
