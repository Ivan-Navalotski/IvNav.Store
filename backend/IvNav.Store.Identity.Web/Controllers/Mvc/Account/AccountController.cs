using IdentityServer4.Services;
using IdentityServer4.Stores;
using IvNav.Store.Identity.Core.Commands.User;
using IvNav.Store.Identity.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using IvNav.Store.Identity.Web.Extensions;
using IdentityServer4;

namespace IvNav.Store.Identity.Web.Controllers.Mvc.Account;

public partial class AccountController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IMediator _mediator;

    public AccountController(IIdentityServerInteractionService interaction, IClientStore clientStore, IMediator mediator)
    {
        _interaction = interaction;
        _clientStore = clientStore;
        _mediator = mediator;
    }

    [Authorize]
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
    public async Task<IActionResult> Login(string? returnUrl)
    {
        var redirect = await GetRedirect(returnUrl);
        if (redirect != null)
        {
            return redirect;
        }

        var vm = await GetLoginViewModelAsync(returnUrl).ConfigureAwait(true);
        return View(vm);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] SignInViewModel viewModel, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(viewModel.ReturnUrl)) viewModel.ReturnUrl = "~/";

        var response = await _mediator.Send(new SignInUserRequest(viewModel.Email!, viewModel.Password!), cancellationToken);

        if (!response.Succeeded)
        {
            ModelState.AddErrors(response.Errors);
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        await SignInCookieAsync(response.Claims!, cancellationToken);

        return Redirect(viewModel.ReturnUrl);
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await HttpContext.SignOutAsync();
        var redirect = await GetRedirect(returnUrl);
        return redirect!;
    }

    /// <summary>
    /// EmailConfirmed
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult EmailConfirmed()
    {
        return View();
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Register(string? returnUrl)
    {
        var redirect = await GetRedirect(returnUrl);
        if (redirect != null)
        {
            return redirect;
        }

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        var viewModel = new RegisterViewModel
        {
            ReturnUrl = returnUrl,
            IsNativeClient = context?.IsNativeClient() ?? false,
        };
        return View(viewModel);
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel, CancellationToken cancellationToken)
    {
        if (viewModel.Password != viewModel.ConfirmPassword)
        {
            ModelState.AddModelError("Password", "ConfirmPasswordNotMatch");
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var confirmationLink = GetHost + Url.RouteUrl(nameof(Api.AccountApiController.ConfirmEmail));
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

                var response = await _mediator.Send(new SignInExternalUserRequest(claims, provider), cancellationToken);
                if (!response.Succeeded)
                {
                    throw new Exception("External authentication error");
                }

                await SignInCookieAsync(response.Claims!, cancellationToken);
                return Redirect(returnUrl);
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
}
