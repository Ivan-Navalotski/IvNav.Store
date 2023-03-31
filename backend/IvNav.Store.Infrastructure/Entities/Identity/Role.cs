using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Infrastructure.Entities.Identity;

public class Role : IdentityRole<string>
{
    public string Description { get; private set; } = null!;
}
