using System.Diagnostics;
using System.Text.Json;
using IvNav.Store.Setup.Attributes;
using IvNav.Store.Setup.Exceptions;
using IvNav.Store.Setup.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace IvNav.Store.Setup.Middleware;

public class RequiredHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public RequiredHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            if (context.Features
                    .Get<IEndpointFeature>()?.Endpoint?.Metadata
                    .FirstOrDefault(m => m is RequestHeadersAttribute) is RequestHeadersAttribute attribute)
            {
                CheckHeaders(context.Request.Headers, attribute.Headers);
            }

            await _next(context);
        }
        catch (RequestHeaderException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new UnhandledExceptionResponseDto
            {
                Message = e.Message,
                TraceId = Activity.Current!.Id!
            });
        }
    }

    /// <summary>
    /// Check headers
    /// </summary>
    /// <exception cref="RequestHeaderException"></exception>
    private void CheckHeaders(IHeaderDictionary headers, IEnumerable<RequestHeaderData> endpointHeaders)
    {

        foreach (var headerData in endpointHeaders)
        {
            if (!headers.TryGetValue(headerData.HeaderName, out var headerValue) && headerData.IsRequired)
            {
                throw new RequestHeaderException(headerData, $"Header {headerData.HeaderName} is missing");
            }

            if (headerValue.Count > 0 && headerData.Type != null)
            {
                try
                {
                    JsonSerializer.Deserialize(JsonSerializer.Serialize(headerValue.ToString()), headerData.Type);
                }
                catch (Exception)
                {
                    throw new RequestHeaderException(headerData, $"Header {headerData.HeaderName} has incorrect type. Should be {headerData.Type.Name}");
                }
            }
        }
    }
}
