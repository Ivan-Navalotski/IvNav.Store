using IvNav.Store.Identity.Infrastructure.Entities;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Identity.Infrastructure.Abstractions.Contexts;

public interface IAppDbContext : IContextBase
{
    DbSet<UserExternalProviderLink> UserExternalProviderLinks { get; }
}
