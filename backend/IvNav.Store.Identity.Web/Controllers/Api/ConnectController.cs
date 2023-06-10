using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace IvNav.Store.Identity.Web.Controllers.Api;

[ApiController]
[Route("connect")]
public class ConnectController : ControllerBase
{
    // !!!
    // Stub class for swagger. The call is made via IdS4

    private const string TokenDescription =
        "<p/>" +
        "<b>Login</b><br/>" +
        "The token endpoint can be used to programmatically request tokens. <br/>" +
        "It supports the <i>password</i> and <i>api-key</i> grant types.<br/>" +
        "<b><i>Example:</i></b><br/>" +
        "POST /connect/token <br/>" +
        "CONTENT-TYPE application/x-www-form-urlencoded <br/>" +
        "client_id=client1&client_secret=secret&grant_type=authorization_code <br/>";

    private const string RefreshTokenDescription =
        "<p/>" +
        "<b>RefreshToken</b><br/>" +
        "To get a new access token, you send the refresh token to the token endpoint.<br/>" +
        "<b><i>Example:</i></b><br/>" +
        "POST /connect/token <br/>" +
        "CONTENT-TYPE application/x-www-form-urlencoded <br/>" +
        "client_id=client1&client_secret=secret&grant_type=refresh_token&refresh_token=hdh922<br/>";


    /// <summary>
    /// Get token
    /// </summary>
    /// <returns></returns>
    [HttpPost("token")]
    [Consumes("application/x-www-form-urlencoded")]
    [SwaggerOperation("Получение токена", TokenDescription + RefreshTokenDescription)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Error", typeof(ConnectTokenErrorResponse))]
    public IActionResult Token([FromForm] ConnectTokenRequest? model)
    {
        if (model != null) return Ok();
        return BadRequest();
    }

    public class ConnectTokenRequest
    {
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }

    public class ConnectTokenErrorResponse
    {
        [JsonPropertyName("error")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("error_description")]
        public string Description { get; set; } = null!;
    }
}
