using IvNav.Store.Identity.Core.Queries.Error;
using IvNav.Store.Identity.Web.ViewModels.Home;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IvNav.Store.Identity.Web.Controllers.Mvc;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Error(string? errorId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ReadErrorRequest(errorId), cancellationToken);

        var viewModel = new ErrorViewModel(response.Error)
        {
            Description = response.Description,
        };
        return View(viewModel);
    }
}
