using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Infrastructure.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public bool NeedSetupPassword { get; set; }
    public List<UserExternalProviderLink> ExternalProviderLinks { get; set; } = null!;
}
