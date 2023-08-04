using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Extensions;
using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Identity.Core.Helpers;

internal class SignInManager : ISignInManager
{
    private readonly SignInManager<User> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _eventService;

    public SignInManager(SignInManager<User> signInManager, IIdentityServerInteractionService interaction, IEventService eventService)
    {
        _signInManager = signInManager;
        _interaction = interaction;
        _eventService = eventService;
    }

    public async Task<ClientInfoModel?> GetClientInfoModel(string? returnUrl, CancellationToken cancellationToken)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context == null) return null;

        return new ClientInfoModel(
            context.Client.ClientName ?? "Unnamed client",
            context.Client.EnableLocalLogin,
            context.Client.LogoUri);
    }

    public async Task<bool> IsInAuthorizationContext(string? returnUrl, CancellationToken cancellationToken)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        return context != null && !context.IsLocalUrl();
    }

    public async Task<string?> GrantConsent(string? returnUrl, CancellationToken cancellationToken)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context != null)
        {
            await _interaction.GrantConsentAsync(context, new ConsentResponse
            {
                RememberConsent = true,
                ScopesValuesConsented = context.ValidatedResources.RawScopeValues,
            });

            return context.RedirectUri;
        }

        return null;
    }

    public async Task<UserResultModel> SignIn(User user, CancellationToken cancellationToken)
    {
        await _signInManager.SignInAsync(user, false);

        var context = await _interaction.GetAuthorizationContextAsync(null);

        await _eventService.RaiseAsync(new UserLoginSuccessEvent(
            user.Email,
            user.Id.ToString(),
            user.UserName,
            clientId: context?.Client.ClientId));

        return new UserResultModel(user);
    }

    public async Task SignOut(User user, CancellationToken cancellationToken)
    {
        await _eventService.RaiseAsync(new UserLogoutSuccessEvent(user.Id.ToString(), user.UserName));

        await _signInManager.SignOutAsync();
    }
}
