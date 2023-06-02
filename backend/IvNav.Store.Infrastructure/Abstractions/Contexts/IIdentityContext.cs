using IvNav.Store.Infrastructure.Abstractions.Contexts.Base;
using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Infrastructure.Abstractions.Contexts;

public interface IIdentityContext : IContextBase
{
    DbSet<UserExternalProviderLink> UserExternalProviderLinks { get; }
}
