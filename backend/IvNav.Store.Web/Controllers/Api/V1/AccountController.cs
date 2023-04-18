using IvNav.Store.Core.Commands.User;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Web.Models.V1.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Web.Controllers.Api.V1;

/// <summary>
/// Auth
/// </summary>
public class AccountController : ApiControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Account controller
    /// </summary>
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromForm] LoginRequestDto requestDto)
    {
        var response = await _mediator.Send(new SignInUserRequest(requestDto.Email!, requestDto.Password!));

        if (response == SignInUserResponse.Error)
        {
            return BadRequest();
        }

        var responseDto = new LoginResponseDto
        {
            Token = response.Token!
        };

        return Ok(responseDto);
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
}
