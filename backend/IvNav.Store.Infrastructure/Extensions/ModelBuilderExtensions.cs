using IvNav.Store.Common.Identity;
using IvNav.Store.Infrastructure.Constants;
using IvNav.Store.Infrastructure.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Infrastructure.Extensions;

internal static class ModelBuilderExtensions
{
    public static void SetupTenantEntity(this ModelBuilder modelBuilder)
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

    public static void SetupTraceableProperties(this ModelBuilder modelBuilder)
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
