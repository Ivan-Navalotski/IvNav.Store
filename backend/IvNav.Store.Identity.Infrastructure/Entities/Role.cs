using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Identity.Infrastructure.Entities;

public class Role : IdentityRole<Guid>
{
    public string Description { get; private set; } = null!;
}
