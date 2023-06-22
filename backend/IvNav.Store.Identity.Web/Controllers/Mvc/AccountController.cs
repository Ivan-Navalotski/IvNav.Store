using Duende.IdentityServer;
using IvNav.Store.Identity.Core.Commands.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using IvNav.Store.Identity.Web.Extensions;
using Duende.IdentityServer.Extensions;
using IvNav.Store.Identity.Core.Queries.User;
using IvNav.Store.Identity.Web.ViewModels.Account;

namespace IvNav.Store.Identity.Web.Controllers.Mvc;

[Authorize]
public class AccountController : Controller
{
    private static readonly string HomeUrl = "~/";

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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl, CancellationToken cancellationToken)
    {
        var viewModel = new SignInViewModel
        {
            ReturnUrl = returnUrl,
        };

        var redirect = await GetRedirectToConsent(returnUrl, cancellationToken);
        return redirect ?? View(viewModel);
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

        var redirect = await GetRedirectToConsent(viewModel.ReturnUrl, cancellationToken);
        return redirect ?? Redirect(HomeUrl);
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SignOutUserRequest(), cancellationToken);

        return Redirect(returnUrl ?? HomeUrl);
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
    public async Task<IActionResult> Register(string? returnUrl, CancellationToken cancellationToken)
    {
        var viewModel = new RegisterViewModel
        {
            ReturnUrl = returnUrl,
        };

        var redirect = await GetRedirectToConsent(returnUrl, cancellationToken);

        return redirect ?? View(viewModel);
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

                var redirect = await GetRedirectToConsent(returnUrl, cancellationToken);
                return redirect ?? Redirect(returnUrl ?? HomeUrl);
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

    [HttpGet]
    public async Task<IActionResult> ConsentAuthorization(string? returnUrl, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ReadAuthClientRequest(returnUrl), cancellationToken);

        var viewModel = new ConsentAuthorizationViewModel
        {
            ReturnUrl = returnUrl,
            LogoUri = response.Result?.LogoUri,
            ClientName = response.Result?.ClientName,
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConsentAuthorization(ConsentAuthorizationViewModel viewModel, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GrantConsentRequest(viewModel.ReturnUrl), cancellationToken);

        if (response.Succeeded)
        {
            //return View("Redirect", new RedirectViewModel { ReturnUrl = viewModel.ReturnUrl! });
            //return View("Redirect", new RedirectViewModel { ReturnUrl = "http://localhost:4200" });
            //return Redirect(viewModel.ReturnUrl!);
            return Redirect("http://localhost:4200");
        }

        return View(viewModel);
    }

    private async Task<IActionResult?> GetRedirectToConsent(string? returnUrl, CancellationToken cancellationToken)
    {
        var isAuthorizationContext = await _mediator.Send(new ReadIsInAuthorizationContextRequest(returnUrl), cancellationToken);
        if (HttpContext.User.IsAuthenticated() && isAuthorizationContext.Value)
        {
            return RedirectToAction("ConsentAuthorization", "Account", new { returnUrl });
        }

        return null;
    }

    //private IActionResult GetSuccessRedirectResult(GrantConsentResponse response)
    //{
    //    var returnUrl = response.ReturnUrl;
    //    if (response.IsLocalUrl)
    //    {
    //        returnUrl = string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl;
    //        return Redirect(returnUrl);
    //    }

    //    HttpContext.Response.StatusCode = 200;
    //    HttpContext.Response.Headers["Location"] = "";
    //    return View("Redirect", new RedirectViewModel { ReturnUrl = returnUrl! });
    //}

    private string GetHost => $"{Request.Scheme}://{Request.Host}";
}
