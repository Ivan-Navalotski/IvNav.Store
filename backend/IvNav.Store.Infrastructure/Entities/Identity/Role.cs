using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Infrastructure.Entities.Identity;

public class Role : IdentityRole<Guid>
{
    public string Description { get; private set; } = null!;
}
