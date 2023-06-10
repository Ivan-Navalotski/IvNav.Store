using IvNav.Store.Setup.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Setup.Controllers.Base;

/// <summary>
/// Base api controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(UnhandledExceptionResponseDto))]

public class ApiControllerBase : ControllerBase
{

    protected string GetHost => $"{Request.Scheme}://{Request.Host}";

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
            var currentValue = Response.Headers[headerName]
                .Where(i => !string.IsNullOrEmpty(i))
                .Select(i => i!)
                .ToList();
            Response.Headers.Remove(headerName);
            currentValue.Add(newValue);
            newValue = string.Join(",", currentValue.Select(i => i.Trim()).ToArray());
        }
        Response.Headers.Add(headerName, newValue);
    }
}
