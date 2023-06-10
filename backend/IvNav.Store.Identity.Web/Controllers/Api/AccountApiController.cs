using AutoMapper;
using IvNav.Store.Identity.Core.Commands.User;
using IvNav.Store.Identity.Core.Queries.User;
using IvNav.Store.Identity.Web.Extensions;
using IvNav.Store.Identity.Web.Models;
using IvNav.Store.Setup.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Identity.Web.Controllers.Api;

/// <summary>
/// Auth
/// </summary>
[Route("api/Account")]
public class AccountApiController : ApiControllerBaseSecure
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Account controller
    /// </summary>
    public AccountApiController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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

        if (!response.Succeeded)
        {
            ModelState.AddErrors(response.Errors);
            return ValidationProblem(ModelState);
        }

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
    /// Get account information
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserInfoResponseDto))]
    public async Task<IActionResult> Info(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ReadUserRequest(UserId), cancellationToken);

        var responseDto = _mapper.Map<UserInfoResponseDto>(response.Item!);

        return Ok(responseDto);
    }
}
