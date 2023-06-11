using Duende.IdentityServer.Models;

namespace IvNav.Store.Identity.Core.Extensions;

internal static class AuthorizationRequestExtensions
{
    /// <summary>
    /// Checks if the redirect URI is for a native client.
    /// </summary>
    /// <returns></returns>
    public static bool IsLocalUrl(this AuthorizationRequest context)
    {
        return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
               && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
    }
}
