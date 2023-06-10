using AutoMapper;
using IvNav.Store.Identity.Core.Queries.User;
using IvNav.Store.Identity.Web.Models.V1.User;
using IvNav.Store.Setup.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Identity.Web.Controllers.Api.V1;

public class UserController : ApiControllerBaseSecure
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get user information
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("UserInfo")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserInfoResponseDto))]
    public async Task<IActionResult> UserInfo(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ReadUserRequest(UserId), cancellationToken);

        var responseDto = _mapper.Map<UserInfoResponseDto>(response.Item!);

        return Ok(responseDto);
    }
}
