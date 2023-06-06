using IvNav.Store.Common.Identity;
using Microsoft.EntityFrameworkCore;
using IvNav.Store.Infrastructure.Entities;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Infrastructure.Constants;
using IvNav.Store.Infrastructure.Entities.Abstractions;
using IvNav.Store.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using IvNav.Store.Infrastructure.Extensions;

namespace IvNav.Store.Infrastructure.Contexts;

internal class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private const string Schema = "dbo";

    public DbSet<Product> Products { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);

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
}
