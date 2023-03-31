using IvNav.Store.Setup.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Setup.Controllers.Base;

/// <summary>
/// Base api controller
/// </summary>
[ApiController]
// ReSharper disable once RouteTemplates.ControllerRouteParameterIsNotPassedToMethods
// ReSharper disable once RouteTemplates.RouteParameterConstraintNotResolved
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(UnhandledExceptionResponseDto))]

public class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Add Header X-Total-Count
    /// </summary>
    protected void AddXTotalCountHeaderToResponse(long total)
    {
        Response.Headers.Add("X-Total-Count", total.ToString());

        const string headerName = "Access-Control-Expose-Headers";

        var newValue = "X-Total-Count";
        if (Response.Headers.ContainsKey(headerName))
        {
            var currentValue = Response.Headers[headerName].ToList();
            Response.Headers.Remove(headerName);
            currentValue.Add(newValue);
            newValue = string.Join(",", currentValue.Select(i => i.Trim()).ToArray());
        }
        Response.Headers.Add(headerName, newValue);
    }
}
