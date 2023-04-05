using IvNav.Store.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Core.Configurations;

public static class RegisterCoreConfiguration
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbDependencies(configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatRAssembly>());

        return services;
    }

    internal class MediatRAssembly
    {

    }
}
