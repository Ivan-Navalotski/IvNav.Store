using Microsoft.AspNetCore.Http;

namespace IvNav.Store.Setup.Middleware;

/// <summary>
/// OperationCanceledMiddleware
/// </summary>
public class OperationCanceledMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="next"></param>
    public OperationCanceledMiddleware(
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
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            if (context.RequestAborted.IsCancellationRequested)
            {
                // Client Closed Request
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\": \"Request was canceled by the client.\"}");
            }
            else
            {
                throw;
            }
        }
    }
}
