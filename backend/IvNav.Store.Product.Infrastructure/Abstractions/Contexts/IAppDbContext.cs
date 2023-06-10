using Microsoft.EntityFrameworkCore;
using IvNav.Store.Infrastructure.Abstractions.Contexts;

namespace IvNav.Store.Product.Infrastructure.Abstractions.Contexts;

public interface IAppDbContext : IContextBase
{
    DbSet<Entities.Product> Products { get; }
}
