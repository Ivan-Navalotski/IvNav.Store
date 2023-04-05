using System.Diagnostics;
using IvNav.Store.Common.Extensions;
using IvNav.Store.Setup.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IvNav.Store.Setup.Middleware;

/// <summary>
/// UnhandledExceptionMiddleware
/// </summary>
public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="next"></param>
    public UnhandledExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invoke
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public async Task Invoke(HttpContext context, ILogger<UnhandledExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            e
                .FlattenHierarchy()
                .ToList()
                .ForEach(x => logger.LogError("{message}", x));

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new UnhandledExceptionResponseDto
            {
                Message = "An unexpected fault happened. Please, contact to administrator.",
                TraceId = Activity.Current!.Id!
            });
        }
    }
}
