using System.Diagnostics;
using IvNav.Store.Mail.Core.Commands.Email;
using IvNav.Store.Mail.Web.Models.V1;
using IvNav.Store.Setup.Attributes;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Web.Mail.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IvNav.Store.Mail.Web.Controllers.V1
{
    public class MailsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromBody] SendEmailRequestDto request, CancellationToken cancellationToken)
        {

            await _mediator.Send(new SendEmailRequest(request.Email!, request.Title!, request.Body!), cancellationToken);

            return NoContent();
        }
    }
}
