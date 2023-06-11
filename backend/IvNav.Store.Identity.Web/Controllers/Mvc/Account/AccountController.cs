using Duende.IdentityServer;
using IvNav.Store.Identity.Core.Commands.User;
using IvNav.Store.Identity.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using IvNav.Store.Identity.Web.Extensions;
using Duende.IdentityServer.Extensions;
using IvNav.Store.Identity.Core.Queries.User;

namespace IvNav.Store.Identity.Web.Controllers.Mvc.Account;

[Authorize]
public class AccountController : Controller
{
    private readonly IMediator _mediator;

    public AccountController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        var response = await _mediator.Send(new ReadIsValidReturnUrlRequest(returnUrl));
        if (!response.Succeeded)
        {
            return GetIncorrectRedirectUrlResult(returnUrl);
        }
        if (HttpContext.User.IsAuthenticated())
        {
            return GetSuccessRedirectResult(returnUrl, response.IsLocalUrl);
        }

        var vm = await GetLoginViewModelAsync(returnUrl, response.IsLocalUrl).ConfigureAwait(true);
        return View(vm);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] SignInViewModel viewModel, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new SignInUserRequest(viewModel.Email!, viewModel.Password!, viewModel.RememberMe, viewModel.ReturnUrl), cancellationToken);

        if (!response.Succeeded)
        {
            ModelState.AddErrors(response.Errors);
            return View(viewModel);
        }

        return GetSuccessRedirectResult(viewModel.ReturnUrl, response.IsLocalUrl);
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SignOutUserRequest(), cancellationToken);

        return GetSuccessRedirectResult(returnUrl, true);
    }

    /// <summary>
    /// Confirm email
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ConfirmEmail(string token, string? returnUrl)
    {
        return !string.IsNullOrEmpty(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("EmailConfirmed");
    }

    /// <summary>
    /// EmailConfirmed
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult EmailConfirmed()
    {
        return View();
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string? returnUrl)
    {
        var response = await _mediator.Send(new ReadIsValidReturnUrlRequest(returnUrl));
        if (!response.Succeeded)
        {
            return GetIncorrectRedirectUrlResult(returnUrl);
        }
        if (HttpContext.User.IsAuthenticated())
        {
            return GetSuccessRedirectResult(returnUrl, response.IsLocalUrl);
        }

        var viewModel = new RegisterViewModel
        {
            ReturnUrl = returnUrl,
            IsLocalUrl = response.IsLocalUrl,
        };
        return View(viewModel);
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel viewModel, CancellationToken cancellationToken)
    {
        var confirmationLink = GetHost + Url.RouteUrl(nameof(ConfirmEmail));
        var confirmationReturnUrl = GetHost + Url.RouteUrl(nameof(EmailConfirmed));

        var response = await _mediator.Send(
            new RegisterUserRequest(
                viewModel.Email!,
                viewModel.Password!,
                confirmationLink,
                confirmationReturnUrl),
            cancellationToken);

        if (!response.Succeeded)
        {
            ModelState.AddErrors(response.Errors);
            return View(viewModel);
        }

        return RedirectToAction("Registered");
    }

    /// <summary>
    /// Registered
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registered()
    {
        return View();
    }

    /// <summary>
    /// External signin callback
    /// </summary>
    /// <param name="provider">Available values: Google</param>
    /// <param name="returnUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SignInExternal(string provider, string? returnUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        var auth = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (auth.Succeeded)
        {
            var externalUser = auth.Principal;
            if (externalUser.Identity!.AuthenticationType == provider)
            {
                var claims = externalUser.Claims.ToList();

                var response = await _mediator.Send(new SignInExternalUserRequest(claims, provider, returnUrl), cancellationToken);
                if (!response.Succeeded)
                {
                    throw new Exception("External authentication error");
                }

                return GetSuccessRedirectResult(returnUrl, response.IsLocalUrl);
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

    private IActionResult GetIncorrectRedirectUrlResult(string? returnUrl)
    {
        return View("IncorrectRedirectUrl", new IncorrectRedirectUrlViewModel { ReturnUrl = returnUrl });
    }

    private IActionResult GetSuccessRedirectResult(string? returnUrl, bool isLocalUrl)
    {
        if (isLocalUrl)
        {
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl;
            return Redirect(returnUrl);
        }

        HttpContext.Response.StatusCode = 200;
        HttpContext.Response.Headers["Location"] = "";
        return View("Redirect", new RedirectViewModel { ReturnUrl = returnUrl! });
    }

    private async Task<SignInViewModel> GetLoginViewModelAsync(string? returnUrl, bool isLocalUrl)
    {
        var response = await _mediator.Send(new ReadAuthClientRequest(returnUrl));

        return new SignInViewModel
        {
            ReturnUrl = returnUrl,
            IsLocalUrl = isLocalUrl,

            ClientName = response.Result?.ClientName,
            LogoUri = response.Result?.LogoUri,
            EnableLocalLogin = response.Result?.EnableLocalLogin ?? false,
        };
    }

    private string GetHost => $"{Request.Scheme}://{Request.Host}";
}
