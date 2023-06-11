using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Identity.Infrastructure.Entities;

public class User : IdentityUser<Guid>
{
    public bool NeedSetupPassword { get; set; }

    public List<UserExternalProviderLink> ExternalProviderLinks { get; set; } = null!;
}
