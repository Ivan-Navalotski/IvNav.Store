using Microsoft.AspNetCore.Http;
using IvNav.Store.Common.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            var token = await context.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme);
            IdentityState.SetCurrent(context.User.Claims, token);
        }

        await _next(context);
    }
}
