using IdentityServer4.Stores;
using IvNav.Store.Identity.Web.Extensions;
using IvNav.Store.Identity.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IdentityServer4;
using IvNav.Store.Common.Constants;

namespace IvNav.Store.Identity.Web.Controllers.Mvc.Account;

public partial class AccountController
{
    private async Task<IActionResult?> GetRedirect(string? returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        // check if we are in the context of an authorization request
        if (User.Identity?.IsAuthenticated != true || context != null || _interaction.IsValidReturnUrl(returnUrl))
        {
            return null;
        }

        returnUrl = string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl;

        if (context != null)
        {
            return context.IsNativeClient()
                ? LoadingPage("Redirect", returnUrl)
                : Redirect(returnUrl);
        }

        // request for a local page
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return Redirect(returnUrl);
    }

    private IActionResult LoadingPage(string viewName, string redirectUri)
    {
        HttpContext.Response.StatusCode = 200;
        HttpContext.Response.Headers["Location"] = "";

        return View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
    }

    private async Task<SignInViewModel> GetLoginViewModelAsync(string? returnUrl)
    {
        var clientName = string.Empty;
        var allowLocal = true;

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
                clientName = client.ClientName;
            }
        }

        return new SignInViewModel
        {
            IsValidReturnUrl = _interaction.IsValidReturnUrl(returnUrl),
            ReturnUrl = returnUrl,
            Email = context?.LoginHint,
            EnableLocalLogin = allowLocal,
            ClientName = clientName,
            IsNativeClient = context?.IsNativeClient() ?? false,
        };
    }

    private async Task SignInCookieAsync(IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        var claimsCollection = claims.ToList();

        var user = new IdentityServerUser(claimsCollection.First(i => i.Type == ClaimIdentityConstants.UserIdClaimType).Value)
        {
            AdditionalClaims = claimsCollection,
        };

        cancellationToken.ThrowIfCancellationRequested();

        await HttpContext.SignInAsync(user);
    }

    private string GetHost => $"{Request.Scheme}://{Request.Host}";
}
