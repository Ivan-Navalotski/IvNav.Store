namespace IvNav.Store.Identity.Web.ViewModels
{
    public class SignedOutViewModel
    {
        public string? PostLogoutRedirectUri { get; set; }

        public string ClientName { get; set; } = null!;

        public string? SignOutIframeUrl { get; set; }

        public bool AutomaticRedirectAfterSignOut { get; set; }

        public string LogoutId { get; set; } = null!;

        public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;

        public string? ExternalAuthenticationScheme { get; set; } = null!;
    }
}
