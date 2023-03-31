using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace IvNav.Store.Setup.Controllers.Base;

/// <summary>
/// Base secure controller
/// </summary>
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
[SwaggerResponse(StatusCodes.Status403Forbidden)]
public abstract class ApiControllerBaseSecure : ApiControllerBase
{
    /// <summary>
    /// UserId
    /// </summary>
    protected Guid UserId => GetUserId();

    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Guid.Parse(userIdString);
    }
}
