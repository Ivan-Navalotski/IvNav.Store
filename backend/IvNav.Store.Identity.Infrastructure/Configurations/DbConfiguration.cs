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
    private const string ConnectionStringName = "DbConnection";

    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Transient);


        services
            .AddIdentity<User, Role>(options =>
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

    public static IIdentityServerBuilder AddIdentityServerContext(this IIdentityServerBuilder builder,
        IConfiguration configuration, TimeSpan tokenCleanupInterval)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        var assemblyName = typeof(DbConfiguration).Assembly.FullName;

        // Adds the config data from DB (clients, resources)
        return builder
                .AddAspNetIdentity<User>()
                .AddConfigurationStore(o =>
                {
                    o.ConfigureDbContext = contextOptionsBuilder =>
                    {
                        contextOptionsBuilder.UseSqlServer(
                            connectionString,
                            bo => bo.MigrationsAssembly(assemblyName));
                    };
                })
                // Adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(o =>
                {
                    o.ConfigureDbContext = contextOptionsBuilder =>
                    {
                        contextOptionsBuilder.UseSqlServer(
                            connectionString,
                            bo => bo.MigrationsAssembly(assemblyName));
                    };

                    // Enables automatic token cleanup. this is optional.
                    o.EnableTokenCleanup = true;
                    o.TokenCleanupInterval = (int)Math.Round(tokenCleanupInterval.TotalSeconds);
                })
            ;
    }
}
