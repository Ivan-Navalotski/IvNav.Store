using System.Security.Claims;
using IvNav.Store.Core.Commands.User;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Setup.Helpers;
using IvNav.Store.Web.Models.V1.Account;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Web.Controllers.Api.V1;

/// <summary>
/// Auth
/// </summary>
public class AccountController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Account controller
    /// </summary>
    public AccountController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <returns></returns>
    [HttpPost("[action]")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto, [FromQuery] string? confirmationReturnUrl, CancellationToken cancellationToken)
    {
        var confirmationLink = GetHost + Url.RouteUrl(nameof(ConfirmEmail));

        var response = await _mediator.Send(
            new RegisterUserRequest(
                requestDto.Email!,
                requestDto.Password!,
                confirmationLink,
                confirmationReturnUrl),
            cancellationToken);

        if (!response.Succeeded) return ReturnValidationProblem(response.Errors);

        return NoContent();
    }

    /// <summary>
    /// Email confirmation
    /// </summary>
    /// <param name="token"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet("[action]", Name = nameof(ConfirmEmail))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        return Redirect(returnUrl);
    }

    /// <summary>
    /// SignOut
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("[action]")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public new async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();

        return NoContent();
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost("[action]")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromForm] LoginRequestDto requestDto, [FromQuery] string? returnUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        var response = await _mediator.Send(new SignInUserRequest(requestDto.Email!, requestDto.Password!), cancellationToken);

        if (!response.Succeeded) return ReturnValidationProblem(response.Errors);

        await SignInCookieAsync(response.Claims!, cancellationToken);

        return Redirect(returnUrl);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost("SignIn/Token")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInByToken([FromForm] LoginRequestDto requestDto, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new SignInUserRequest(requestDto.Email!, requestDto.Password!), cancellationToken);

        if (!response.Succeeded)
        {
            return BadRequest();
        }

        var responseDto = new LoginResponseDto
        {
            Token = (new JwtHelper(_configuration)).Generate(response.Claims!)
        };

        return Ok(responseDto);
    }

    /// <summary>
    /// External signin callback
    /// </summary>
    /// <param name="provider">Available values: Google</param>
    /// <param name="returnUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("SignIn/External/{provider}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInExternal([FromRoute] string provider, [FromQuery] string? returnUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            if (HttpContext.User.Identity.AuthenticationType == provider)
            {
                var response = await _mediator.Send(new SignInExternalUserRequest(HttpContext.User.Claims.ToList(), provider), cancellationToken);
                if (response.Succeeded)
                {
                    await SignInCookieAsync(response.Claims!, cancellationToken);
                    return Redirect(returnUrl);
                }
            }

            await HttpContext.SignOutAsync();
        }

        var props = new AuthenticationProperties
        {
            Items =
            {
                { "provider", provider },
                { "returnUrl", returnUrl },
            }
        };

        return Challenge(props, provider);
    }

    private async Task SignInCookieAsync(IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        const string authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        var id = new ClaimsIdentity(claims, authScheme, ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        cancellationToken.ThrowIfCancellationRequested();

        await HttpContext.SignInAsync(authScheme, new ClaimsPrincipal(id));
    }

    private IActionResult ReturnValidationProblem(IReadOnlyDictionary<string, string[]> errors)
    {
        foreach (var keyValuePair in errors)
        {
            foreach (var error in keyValuePair.Value)
            {
                ModelState.AddModelError(keyValuePair.Key, error);
            }
        }

        return ValidationProblem(ModelState);
    }
}
