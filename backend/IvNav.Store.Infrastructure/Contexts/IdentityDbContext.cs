using IvNav.Store.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IvNav.Store.Infrastructure.Contexts;

internal class IdentityDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IIdentityContext
{
}
