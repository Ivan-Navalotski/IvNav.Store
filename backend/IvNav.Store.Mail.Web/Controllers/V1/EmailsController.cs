using IvNav.Store.Mail.Core.Commands.Email;
using IvNav.Store.Mail.Web.Models.V1;
using IvNav.Store.Setup.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Mail.Web.Controllers.V1;

public class EmailsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public EmailsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Send([FromBody] SendEmailRequestDto request, CancellationToken cancellationToken)
    {

        await _mediator.Send(new SendEmailRequest(request.Subject!, request.To!, request.Body!), cancellationToken);

        return NoContent();
    }
}
