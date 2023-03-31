using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Infrastructure.Entities.Identity;

public class UserRole : IdentityUserRole<string>
{
    public virtual User User { get; private set; } = null!;
    public virtual Role Role { get; private set; } = null!;
}
