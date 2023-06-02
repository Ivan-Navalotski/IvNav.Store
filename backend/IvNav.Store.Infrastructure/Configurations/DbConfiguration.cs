using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Infrastructure.Contexts;
using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityDbContext = IvNav.Store.Infrastructure.Contexts.IdentityDbContext;

namespace IvNav.Store.Infrastructure.Configurations;

public static class DbConfiguration
{
    public static IServiceCollection AddDbDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnection")),
            ServiceLifetime.Transient);

        services.AddDbContext<IIdentityContext, IdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DbConnection")),
            ServiceLifetime.Transient);


        services
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<Role>()
            .AddUserStore<UserStore<User, Role, IdentityDbContext, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>>()
            .AddRoleStore<RoleStore<Role, IdentityDbContext, Guid, UserRole, IdentityRoleClaim<Guid>>>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, Role>>()
            .AddEntityFrameworkStores<IdentityDbContext>();

        return services;
    }
}
