using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using IvNav.Store.Common.Constants;
using Microsoft.AspNetCore.Authentication;

namespace IvNav.Store.Setup.Middleware;

public class HeaderAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="next"></param>
    public HeaderAuthenticationMiddleware(
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
        if (!(context.User.Identity?.IsAuthenticated ?? false))
        {
            var userIdString = string.Empty;
            var tenantIdString = string.Empty;

            if (context.Request.Headers.TryGetValue(HeaderNames.UserId, out var userIdHeaderValue))
            {
                userIdString = userIdHeaderValue.FirstOrDefault() ?? string.Empty;
            }
            if (context.Request.Headers.TryGetValue(HeaderNames.TenantId, out var tenantIdHeaderValue))
            {
                tenantIdString = tenantIdHeaderValue.FirstOrDefault() ?? string.Empty;
            }

            // Authenticate
            if (!string.IsNullOrEmpty(userIdString))
            {
                var claims = new List<Claim>
                {
                    new(ClaimIdentityConstants.UserIdClaimType, userIdString),
                };

                if (!string.IsNullOrEmpty(tenantIdString))
                {
                    claims.Add(new Claim(ClaimIdentityConstants.TenantIdClaimType, tenantIdString));
                }

                await context.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims)));
            }
        }

        await _next(context);
    }
}
