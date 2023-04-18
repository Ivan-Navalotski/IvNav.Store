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
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            var userIdString = context.User.FindFirstValue(ClaimIdentityConstants.UserIdClaimType)!;
            var tenantIdString = context.User.FindFirstValue(ClaimIdentityConstants.TenantIdClaimType)!;

            IdentityState.SetCurrent(Guid.Parse(userIdString), Guid.Parse(tenantIdString));
        }

        await _next(context);
    }
}
