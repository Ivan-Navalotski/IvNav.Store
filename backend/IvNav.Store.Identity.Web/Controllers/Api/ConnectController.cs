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
        /// <summary>
        /// client identifier (required – Either in the body or as part of the authorization header.)
        /// </summary>
        [JsonPropertyName("client_id")]
        [FromForm(Name = "client_id")]
        public string? ClientId { get; set; }

        /// <summary>
        /// client secret either in the post body, or as a basic authentication header. Optional.
        /// </summary>
        [JsonPropertyName("client_secret")]
        [FromForm(Name = "client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// authorization_code, client_credentials, password, refresh_token, urn:ietf:params:oauth:grant-type:device_code or custom
        /// </summary>
        [JsonPropertyName("grant_type")]
        [FromForm(Name = "grant_type")]
        public string? GrantType { get; set; }

        /// <summary>
        /// one or more registered scopes. If not specified, a token for all explicitly allowed scopes will be issued.
        /// </summary>
        [JsonPropertyName("scope")]
        [FromForm(Name = "scope")]
        public string? Scope { get; set; }

        /// <summary>
        /// required for the authorization_code grant type
        /// </summary>
        [JsonPropertyName("redirect_uri")]
        [FromForm(Name = "redirect_uri")]
        public string? RedirectUri { get; set; }

        /// <summary>
        /// the authorization code (required for authorization_code grant type)
        /// </summary>
        [JsonPropertyName("code")]
        [FromForm(Name = "code")]
        public string? Code { get; set; }

        /// <summary>
        /// resource owner username (required for password grant type)
        /// </summary>
        [JsonPropertyName("username")]
        [FromForm(Name = "username")]
        public string? Username { get; set; }

        /// <summary>
        /// resource owner password (required for password grant type)
        /// </summary>
        [JsonPropertyName("password")]
        [FromForm(Name = "password")]
        public string? Password { get; set; }

        /// <summary>
        /// allows passing in additional authentication related information for the password grant type - identityserver special cases the following proprietary acr_values:
        /// idp:name_of_idp bypasses the login/home realm screen and forwards the user directly to the selected identity provider (if allowed per client configuration)
        /// tenant:name_of_tenant can be used to pass a tenant name to the token endpoint
        /// </summary>
        [JsonPropertyName("acr_values")]
        [FromForm(Name = "acr_values")]
        public string? AcrValues { get; set; }

        /// <summary>
        /// the refresh token (required for refresh_token grant type)
        /// </summary>
        [JsonPropertyName("refresh_token")]
        [FromForm(Name = "refresh_token")]
        public string? RefreshToken { get; set; }

        /// <summary>
        /// the device code (required for urn:ietf:params:oauth:grant-type:device_code grant type)
        /// </summary>
        [JsonPropertyName("device_code")]
        [FromForm(Name = "device_code")]
        public string? DeviceCode { get; set; }
    }

    public class ConnectTokenErrorResponse
    {
        [JsonPropertyName("error")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("error_description")]
        public string Description { get; set; } = null!;
    }
}
