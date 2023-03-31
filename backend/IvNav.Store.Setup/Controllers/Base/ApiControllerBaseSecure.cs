using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using IvNav.Store.Common.Identity;

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
    protected Guid UserId => IdentityState.Current!.UserId;
}
