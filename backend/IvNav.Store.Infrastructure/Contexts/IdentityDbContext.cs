using IvNav.Store.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Infrastructure.Entities.Identity;
using IvNav.Store.Infrastructure.EntityConfigurations.User;
using IvNav.Store.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Infrastructure.Contexts;

internal class IdentityDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
    UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IIdentityContext
{
    private const string Schema = "identity";

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);

        // Configurations
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserExternalProviderLinkConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
    }

    public DbSet<UserExternalProviderLink> UserExternalProviderLinks { get; set; } = null!;

    public async Task<T> BeginTransaction<T>(Func<Task<T>> func, CancellationToken cancellationToken)
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var res = await func.Invoke();

            await transaction.CommitAsync(cancellationToken);

            return res;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
