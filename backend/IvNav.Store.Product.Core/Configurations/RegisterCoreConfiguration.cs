using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Product.Core.Configurations;

public static class RegisterCoreConfiguration
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatRAssembly>());

        return services;
    }

    internal class MediatRAssembly
    {

    }
}
