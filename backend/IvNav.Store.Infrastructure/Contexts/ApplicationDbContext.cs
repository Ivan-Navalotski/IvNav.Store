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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);

        // Configurations
        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        // Setup Props
        SetupTraceableProperties(modelBuilder);
        SetupTenantEntity(modelBuilder);
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

    private static void SetTenantInfo(EntityEntry[] changes)
    {
        if (changes.Length == 0)
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

    private static void LogTraceableData(EntityEntry[] changes)
    {
        if (changes.Length == 0)
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

    private static void SetupTenantEntity(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;

            if (typeof(ITenantEntity).IsAssignableFrom(entityClrType))
            {
                var tenantId = IdentityState.Current?.TenantId;

                var entityBuilder = modelBuilder.Entity(entityClrType);
                entityBuilder.Property<Guid>(ShadowPropertyConstants.TenantId);
                entityBuilder.AddQueryFilter<ITenantEntity>(x => x.TenantId == tenantId);
            }
        }
    }

    private static void SetupTraceableProperties(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;

            if (typeof(ITraceableEntity).IsAssignableFrom(entityClrType))
            {
                var entityBuilder = modelBuilder.Entity(entityClrType);

                entityBuilder
                    .Property<Guid>(ShadowPropertyConstants.CreateUserId)
                    .IsRequired();

                entityBuilder
                    .Property<DateTime>(ShadowPropertyConstants.CreateDateTime)
                    .IsRequired();

                entityBuilder
                    .Property<Guid>(ShadowPropertyConstants.UpdateUserId)
                    .IsRequired();

                entityBuilder
                    .Property<DateTime>(ShadowPropertyConstants.UpdateDateTime)
                    .IsRequired();
            }
        }
    }
}
