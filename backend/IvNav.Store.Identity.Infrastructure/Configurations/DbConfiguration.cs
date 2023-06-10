using IvNav.Store.Identity.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Identity.Infrastructure.Contexts;
using IvNav.Store.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Identity.Infrastructure.Configurations;

public static class DbConfiguration
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Transient);


        services
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<Role>()
            .AddUserStore<UserStore<User, Role, AppDbContext, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>>()
            .AddRoleStore<RoleStore<Role, AppDbContext, Guid, UserRole, IdentityRoleClaim<Guid>>>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, Role>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
