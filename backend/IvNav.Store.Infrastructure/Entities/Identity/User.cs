using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Infrastructure.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string? GivenName { get; set; }

    public string? Surname { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public bool NeedSetupPassword { get; set; }

    public List<UserExternalProviderLink> ExternalProviderLinks { get; set; } = null!;
}
