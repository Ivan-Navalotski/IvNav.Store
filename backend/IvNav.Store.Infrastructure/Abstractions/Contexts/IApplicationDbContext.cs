using IvNav.Store.Infrastructure.Abstractions.Contexts.Base;
using IvNav.Store.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Infrastructure.Abstractions.Contexts;

public interface IApplicationDbContext : IContextBase
{
    DbSet<Product> Products { get; }
}
