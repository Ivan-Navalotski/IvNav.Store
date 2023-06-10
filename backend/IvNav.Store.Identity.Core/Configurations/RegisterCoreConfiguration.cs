using IvNav.Store.Core.Interaction.Configurations;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Helpers;
using IvNav.Store.Identity.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Ardalis.GuardClauses;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;

namespace IvNav.Store.Identity.Core.Configurations;

public static class RegisterCoreConfiguration
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureDependencies(configuration);
        services.AddInteractionDependencies();

        services.AddTransient<IUserManager, UserManager>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(RegisterCoreConfiguration)));

        services.AddIdentityServer(configuration);

        return services;
    }

    private static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var settingsSection = Guard.Against.Null(configuration.GetSection("AuthenticationSettings:IdentityServer"));

        var publicKey = Guard.Against.NullOrEmpty(settingsSection.GetValue<string>("PublicKey"));
        var privateKey = Guard.Against.NullOrEmpty(settingsSection.GetValue<string>("PrivateKey"));

        var cleanupInterval = Guard.Against.Default(settingsSection.GetValue<TimeSpan>("TokenCleanupInterval"));

        // Key
        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        var key = new RsaSecurityKey(rsa);

        services
            .AddIdentityServer()
            .AddSigningCredential(key, IdentityServerConstants.RsaSigningAlgorithm.PS256)
            .AddIdentityServerContext(configuration, cleanupInterval)
            //.AddClientConfigurationValidator<>()
            ;
    }

    public static IApplicationBuilder UseCoreIdentityServer(this IApplicationBuilder app)
    {
        app.UseIdentityServer();

        return app;
    }
}
