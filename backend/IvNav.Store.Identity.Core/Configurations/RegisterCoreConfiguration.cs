using IvNav.Store.Core.Interaction.Configurations;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Helpers;
using IvNav.Store.Identity.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Identity.Core.Configurations;

public static class RegisterCoreConfiguration
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureDependencies(configuration);
        services.AddInteractionDependencies();

        services.AddTransient<IUserManager, UserManager>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatRAssembly>());

        return services;
    }

    internal class MediatRAssembly
    {

    }
}
