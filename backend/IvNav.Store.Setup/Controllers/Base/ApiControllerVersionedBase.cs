using Microsoft.AspNetCore.Mvc;

namespace IvNav.Store.Setup.Controllers.Base;

/// <summary>
/// Base versioned api controller
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
internal class ApiControllerVersionedBase : ApiControllerBase
{
}
