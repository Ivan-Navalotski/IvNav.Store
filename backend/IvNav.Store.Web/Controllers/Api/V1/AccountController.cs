using System.Security.Claims;
using IvNav.Store.Core.Commands.User;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Setup.Helpers;
using IvNav.Store.Web.Helpers;
using IvNav.Store.Web.Models.V1.Account;
using MediatR;
using Microsoft.AspNetCore.Authentication;
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
    [HttpPost]
    [Route("[action]")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
    {
        var response = await _mediator.Send(new RegisterUserRequest(requestDto.Email!, requestDto.Password!));

        if (response == RegisterUserResponse.Error)
        {
            return BadRequest();
        }

        return NoContent();
    }

    /// <summary>
    /// SignOut
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("SignOut")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync();

        return NoContent();
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SignIn")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInAsync([FromForm] LoginRequestDto requestDto, [FromQuery] string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        var response = await _mediator.Send(new SignInUserRequest(requestDto.Email!, requestDto.Password!));

        if (response == SignInUserResponse.Error)
        {
            return BadRequest();
        }

        var id = new ClaimsIdentity(response.Claims!, AuthHelper.MainAuthScheme, ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(AuthHelper.MainAuthScheme, new ClaimsPrincipal(id));

        return NoContent();
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("SignIn/token")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInTokenAsync([FromForm] LoginRequestDto requestDto)
    {
        var response = await _mediator.Send(new SignInUserRequest(requestDto.Email!, requestDto.Password!));

        if (response == SignInUserResponse.Error)
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
    /// <returns></returns>
    [HttpGet]
    [Route("SignIn/external/{provider}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInExternalAsync([FromRoute] string provider, [FromQuery] string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            if (HttpContext.User.Identity.AuthenticationType == provider)
            {
                return Redirect(returnUrl);
            }

            await HttpContext.SignOutAsync();
        }

        var props = new AuthenticationProperties
        {
            Items =
            {
                {"provider", provider},
                {"returnUrl", returnUrl},
            }
        };

        return Challenge(props, provider);
    }
}
