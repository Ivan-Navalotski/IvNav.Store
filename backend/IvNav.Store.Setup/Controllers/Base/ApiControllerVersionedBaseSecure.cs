using IvNav.Store.Common.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Setup.Controllers.Base;

/// <summary>
/// Base versioned secure controller
/// </summary>
[Authorize]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
[SwaggerResponse(StatusCodes.Status403Forbidden)]
public class ApiControllerVersionedBaseSecure : ApiControllerVersionedBase
{
    /// <summary>
    /// UserId
    /// </summary>
    protected Guid UserId => IdentityState.Current!.UserId;
}
