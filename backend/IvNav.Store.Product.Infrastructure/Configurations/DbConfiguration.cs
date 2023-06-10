using IvNav.Store.Product.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Product.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Product.Infrastructure.Configurations;

public static class DbConfiguration
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            options.UseSqlServer(connectionString),
            ServiceLifetime.Transient);

        return services;
    }
}
