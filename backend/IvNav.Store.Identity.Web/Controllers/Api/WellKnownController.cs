using Microsoft.AspNetCore.Mvc;

namespace IvNav.Store.Identity.Web.Controllers.Api;

[ApiController]
[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    // !!!
    // Stub class for swagger. The call is made via IdS4

    /// <summary>
    /// Get open id configuration
    /// </summary>
    /// <returns></returns>
    [HttpGet("openid-configuration")]
    public IActionResult OpenIdConfiguration()
    {
        return Ok();
    }
}
