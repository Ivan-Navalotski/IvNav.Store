using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using IvNav.Store.Common.Identity;
using Microsoft.AspNetCore.Authorization;

namespace IvNav.Store.Setup.Controllers.Base;

/// <summary>
/// Base secure controller
/// </summary>
[Authorize]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
[SwaggerResponse(StatusCodes.Status403Forbidden)]
public abstract class ApiControllerBaseSecure : ApiControllerBase
{
    /// <summary>
    /// UserId
    /// </summary>
    protected Guid UserId => IdentityState.Current!.UserId;
}
