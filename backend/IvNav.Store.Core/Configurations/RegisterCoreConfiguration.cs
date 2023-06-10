using IvNav.Store.Core.Abstractions.Helpers;
using IvNav.Store.Core.Helpers;
using IvNav.Store.Core.Interaction.Configurations;
using IvNav.Store.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Core.Configurations;

public static class RegisterCoreConfiguration
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbDependencies(configuration);
        services.AddInteractionDependencies();

        services.AddTransient<IUserManager, UserManager>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatRAssembly>());

        return services;
    }

    internal class MediatRAssembly
    {

    }
}
