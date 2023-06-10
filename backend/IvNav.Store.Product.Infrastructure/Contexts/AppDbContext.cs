using IvNav.Store.Common.Identity;
using IvNav.Store.Infrastructure.Constants;
using IvNav.Store.Infrastructure.Entities.Abstractions;
using IvNav.Store.Infrastructure.Extensions;
using IvNav.Store.Product.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Product.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IvNav.Store.Product.Infrastructure.Contexts;

internal class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Entities.Product> Products { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurations
        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        // Setup Props
        modelBuilder.SetupTraceableProperties();
        modelBuilder.SetupTenantEntity();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges();

        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void OnBeforeSaveChanges()
    {
        var changeTracker = ChangeTracker;
        if (!changeTracker.AutoDetectChangesEnabled)
        {
            // ChangeTracker.Entries() calls DetectChanges() if auto detect enabled
            changeTracker.DetectChanges();
        }

        var changes = changeTracker.Entries().ToArray();

        SetTenantInfo(changes);
        LogTraceableData(changes);
    }

    private static void SetTenantInfo(IReadOnlyCollection<EntityEntry> changes)
    {
        if (changes.Count == 0)
        {
            return;
        }

        foreach (var entry in changes)
        {
            if (entry.Entity is ITenantEntity
                && entry.State is EntityState.Added or EntityState.Modified
                && (entry.Property(ShadowPropertyConstants.TenantId).CurrentValue == null || (Guid)entry.Property(ShadowPropertyConstants.TenantId).CurrentValue! == Guid.Empty) &&
                IdentityState.Current?.TenantId != null)
            {
                entry.Property(ShadowPropertyConstants.TenantId).CurrentValue = IdentityState.Current.TenantId;
            }
        }
    }

    private static void LogTraceableData(IReadOnlyCollection<EntityEntry> changes)
    {
        if (changes.Count == 0)
        {
            return;
        }

        var userId = IdentityState.Current?.UserId;
        var utcNow = (object)DateTime.UtcNow;

        foreach (var entry in changes)
        {
            var entity = entry.Entity;
            if (entity is ITraceableEntity)
            {
                var state = entry.State;

                if (state == EntityState.Added)
                {
                    entry.Property(ShadowPropertyConstants.CreateDateTime).CurrentValue = userId;
                    entry.Property(ShadowPropertyConstants.CreateDateTime).CurrentValue = utcNow;
                }

                if (state is EntityState.Added or EntityState.Modified)
                {
                    entry.Property(ShadowPropertyConstants.CreateDateTime).CurrentValue = userId;
                    entry.Property(ShadowPropertyConstants.CreateDateTime).CurrentValue = utcNow;
                }
            }
        }
    }

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
